using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Queries.Common;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using SkiaSharp.QrCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class DownloadQRCodeQueryHandler : IRequestHandler<DownloadQRCodeQuery, byte[]>
    {
        private readonly IAutobotDbContext _context;

        public DownloadQRCodeQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<byte[]> Handle(DownloadQRCodeQuery request, CancellationToken cancellationToken)
        {
            List<ArchiveFile> fileList = new List<ArchiveFile>();
            Guid id = new Guid(request.BatchId);
            var promoCodes = await (from batch in _context.PromoCodeBatch
                                    join codes in _context.PromoCodes
                                    on batch.BatchId equals codes.BatchId
                                    where batch.BatchId == id && batch.IsDeleted == false
                                    && codes.IsDeleted == false
                                    select new PromoCodeDetails
                                    {
                                        PromoCodeNumber = codes.PromoCodeNumber,
                                        PromoCodeId = codes.PromoCodeId,
                                        BrandName = batch.Brand.BrandName,
                                        ExpirationDateTime = batch.ExpirationDateTime,
                                        LoyaltyPoints = batch.LoyaltyPoints
                                    }).ToListAsync();

            foreach (var promocode in promoCodes)
            {
                var text = promocode.PromoCodeNumber + "_" + promocode.PromoCodeId.ToString();
                var qrMap = QrCodeGenerator(text, request.FileType, request.Height, request.Width, request.Color, promocode.LoyaltyPoints.ToString() + " " + "Points", promocode.PromoCodeNumber.ToString());

                fileList.Add(
                    new ArchiveFile
                    {
                        Name = promocode.PromoCodeNumber.ToString(),
                        Extension = "png",
                        FileBytes = qrMap
                    });
            }

            var zip = GeneratePackage(fileList);
            return zip;
        }

        /// <summary>
        /// Create Qr code
        /// Return bytes
        /// </summary>
        /// <param name="qrText"></param>
        /// <returns></returns>
        private Byte[] QrCodeGenerator(string qrText, string fileType, int height, int width, string color, string points, string promocodeNumber)
        {
            using (var generator = new QRCodeGenerator())
            {
                // Generate QrCode
                var qr = generator.CreateQrCode(qrText, ECCLevel.Q);

                // Render to canvas
                var info = new SKImageInfo(width, height);
                using (var surface = SKSurface.Create(info))
                {
                    var canvas = surface.Canvas;
                    canvas.Render(qr, info.Width, info.Height, color, points, promocodeNumber);

                    SKEncodedImageFormat imageFormat;
                    switch (fileType.ToLower())
                    {
                        case "png":
                            imageFormat = SKEncodedImageFormat.Png;
                            break;
                        case "jpeg":
                            imageFormat = SKEncodedImageFormat.Jpeg;
                            break;
                        default:
                            imageFormat = SKEncodedImageFormat.Png;
                            break;
                    }

                    using (var image = surface.Snapshot())
                    using (var data = image.Encode(imageFormat, 100))
                    using (MemoryStream stream = new MemoryStream())
                    {
                        data.SaveTo(stream);
                        return stream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Creates zip folder  of bytes of qr codes
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        private byte[] GeneratePackage(List<ArchiveFile> fileList)
        {
            byte[] result;

            using (var packageStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Create, true))
                {
                    foreach (var virtualFile in fileList)
                    {
                        //Create a zip entry for each attachment
                        var zipFile = archive.CreateEntry(virtualFile.Name + "." + virtualFile.Extension);
                        using (var sourceFileStream = new MemoryStream(virtualFile.FileBytes))
                        using (var zipEntryStream = zipFile.Open())
                        {
                            sourceFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }
                result = packageStream.ToArray();
            }

            return result;
        }
    }
}