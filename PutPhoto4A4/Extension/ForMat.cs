using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutPhoto4A4.Extension
{
    public static class ForMat
    {
        public static Mat RotateImage(this Mat source, double angle)
        {
            if (angle == 90)
                return source.T().Flip(OpenCvSharp.FlipMode.X);
            else if (angle == 180)
                return source.Flip(OpenCvSharp.FlipMode.XY);
            else if (angle == 270)
                return source.T().Flip(OpenCvSharp.FlipMode.Y);
            else
                return source.Clone();
        }
    }

}
