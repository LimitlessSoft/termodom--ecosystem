using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace TD.Web.Common.Contracts.Helpers.Images
{
    public static class ImagesHelper
    {
        public static bool IsImageTypeNotValid(this IFormFile filename)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(filename.OpenReadStream()))
                {
                    UInt16 soi = br.ReadUInt16();
                    UInt16 marker = br.ReadUInt16();

                    Boolean isJpeg = soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
                    Boolean isPng = soi == 0x5089;

                    return !(isJpeg || isPng);
                }
            }
            catch
            {
                return true;
            }
        }

        public static bool isAltValueNotValid(this string alt) =>
            Regex.IsMatch(alt,Constants.RegexValidateAltValuePattern);
    }
}
