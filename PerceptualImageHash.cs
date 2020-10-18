using System;

namespace NutzCode.Libraries.PerceptualImage
{
    public class PerceptualImageHash
    {
        private static readonly float[,] DCT = DCT_Matrix(32);
        private static readonly float[,] DCT_Transpose = DCT_Matrix_Transpose(32);


        private static float[,] DCT_Matrix(int n)
        {
            float[,] matrix = new float[n, n];
            float init = 1F / (float) Math.Sqrt(n);
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    matrix[x, y] = init;
                }
            }

            float c1 = (float) Math.Sqrt(2.0 / n);
            for (int x = 0; x < n; x++)
            {
                for (int y = 1; y < n; y++)
                {
                    float tt = (float) (c1 * Math.Cos(Math.PI / 2 / n * y * (2 * x + 1)));
                    matrix[x, y] = tt;
                }
            }

            return matrix;
        }

        private static float[,] DCT_Matrix_Transpose(int n)
        {
            float[,] matrix = new float[n, n];
            float init = 1F / (float) Math.Sqrt(n);
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                    matrix[x, y] = init;
            }

            float c1 = (float) Math.Sqrt(2.0 / n);
            for (int x = 0; x < n; x++)
            {
                for (int y = 1; y < n; y++)
                {
                    matrix[y, x] = (float) (c1 * Math.Cos(Math.PI / 2 / n * y * (2 * x + 1)));
                }
            }

            return matrix;
        }


        public static byte[] GenerateHashes(float[] data)
        {
            byte[] res = new byte[data.Length >> 3];
            float[] sort = new float[data.Length];
            data.CopyTo(sort, 0);
            Array.Sort(sort);
            int mid = data.Length >> 1;
            float median = (sort[mid - 1] + sort[mid]) / 2;
            int cnt = 0;
            for (int x = 0; x < res.Length; x++)
            {
                int one = 1;
                int r = 0;
                for (int y = 0; y < 8; y++)
                {
                    if (data[cnt++] > median)
                        r |= one;
                    one = one << 1;
                }

                res[x] = (byte) r;
            }

            return res;
        }


        public static byte[] Hash(int[,] image, int width, int height)
        {
            int[,] image2 = new int[width, height];
            int mye = height - 3;
            int mxe = width - 3;


            //blur
            for (int y = 3; y < mye; y++)
            {
                for (int x = 3; x < mxe; x++)
                {
                    int xw = x - 3;
                    int yw = y - 3;
                    int res = image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw++];
                    xw = x - 3;
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw++, yw];
                    res += image[xw, yw];
                    image2[x, y] = res;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width;)
                {
                    int res = 0;
                    for (int ym = -3; ym <= 3; ++ym)
                    {
                        for (int xm = -3; xm <= 3; ++xm)
                        {
                            int xa = x + xm;
                            int ya = y + ym;
                            xa = xa < 0 ? 0 : xa >= width ? width - 1 : xa;
                            ya = ya < 0 ? 0 : ya >= height ? height - 1 : ya;
                            res += image[xa, ya];
                        }
                    }

                    image2[x, y] = res;
                    if (y < 3 || y >= mye)
                        x++;
                    else if (x < 2 || x >= mxe)
                        x++;
                    else
                        x = mxe;
                }
            }


            float[,] sized = new float[32, 32];
            //resize

            int[] off_x = new int[32];
            int[] off_y = new int[32];

            if (width == 32)
            {
                off_x[0] = 0;
                off_x[1] = 1;
                off_x[2] = 2;
                off_x[3] = 3;
                off_x[4] = 4;
                off_x[5] = 5;
                off_x[6] = 6;
                off_x[7] = 7;
                off_x[8] = 8;
                off_x[9] = 9;
                off_x[10] = 10;
                off_x[11] = 11;
                off_x[12] = 12;
                off_x[13] = 13;
                off_x[14] = 14;
                off_x[15] = 15;
                off_x[16] = 16;
                off_x[17] = 17;
                off_x[18] = 18;
                off_x[19] = 19;
                off_x[20] = 20;
                off_x[21] = 21;
                off_x[22] = 22;
                off_x[23] = 23;
                off_x[24] = 24;
                off_x[25] = 25;
                off_x[26] = 26;
                off_x[27] = 27;
                off_x[28] = 28;
                off_x[29] = 29;
                off_x[30] = 30;
                off_x[31] = 31;
            }
            else
            {
                int init = 0;
                off_x[0] = init >> 5;
                init = width;
                off_x[1] = init >> 5;
                init += width;
                off_x[2] = init >> 5;
                init += width;
                off_x[3] = init >> 5;
                init += width;
                off_x[4] = init >> 5;
                init += width;
                off_x[5] = init >> 5;
                init += width;
                off_x[6] = init >> 5;
                init += width;
                off_x[7] = init >> 5;
                init += width;
                off_x[8] = init >> 5;
                init += width;
                off_x[9] = init >> 5;
                init += width;
                off_x[10] = init >> 5;
                init += width;
                off_x[11] = init >> 5;
                init += width;
                off_x[12] = init >> 5;
                init += width;
                off_x[13] = init >> 5;
                init += width;
                off_x[14] = init >> 5;
                init += width;
                off_x[15] = init >> 5;
                init += width;
                off_x[16] = init >> 5;
                init += width;
                off_x[17] = init >> 5;
                init += width;
                off_x[18] = init >> 5;
                init += width;
                off_x[19] = init >> 5;
                init += width;
                off_x[20] = init >> 5;
                init += width;
                off_x[21] = init >> 5;
                init += width;
                off_x[22] = init >> 5;
                init += width;
                off_x[23] = init >> 5;
                init += width;
                off_x[24] = init >> 5;
                init += width;
                off_x[25] = init >> 5;
                init += width;
                off_x[26] = init >> 5;
                init += width;
                off_x[27] = init >> 5;
                init += width;
                off_x[28] = init >> 5;
                init += width;
                off_x[29] = init >> 5;
                init += width;
                off_x[30] = init >> 5;
                init += width;
                off_x[31] = init >> 5;
            }

            if (height == 32)
            {
                off_y[0] = 0;
                off_y[1] = 1;
                off_y[2] = 2;
                off_y[3] = 3;
                off_y[4] = 4;
                off_y[5] = 5;
                off_y[6] = 6;
                off_y[7] = 7;
                off_y[8] = 8;
                off_y[9] = 9;
                off_y[10] = 10;
                off_y[11] = 11;
                off_y[12] = 12;
                off_y[13] = 13;
                off_y[14] = 14;
                off_y[15] = 15;
                off_y[16] = 16;
                off_y[17] = 17;
                off_y[18] = 18;
                off_y[19] = 19;
                off_y[20] = 20;
                off_y[21] = 21;
                off_y[22] = 22;
                off_y[23] = 23;
                off_y[24] = 24;
                off_y[25] = 25;
                off_y[26] = 26;
                off_y[27] = 27;
                off_y[28] = 28;
                off_y[29] = 29;
                off_y[30] = 30;
                off_y[31] = 31;
            }
            else
            {
                int init = 0;
                off_y[0] = init >> 5;
                init = height;
                off_y[1] = init >> 5;
                init += height;
                off_y[2] = init >> 5;
                init += height;
                off_y[3] = init >> 5;
                init += height;
                off_y[4] = init >> 5;
                init += height;
                off_y[5] = init >> 5;
                init += height;
                off_y[6] = init >> 5;
                init += height;
                off_y[7] = init >> 5;
                init += height;
                off_y[8] = init >> 5;
                init += height;
                off_y[9] = init >> 5;
                init += height;
                off_y[10] = init >> 5;
                init += height;
                off_y[11] = init >> 5;
                init += height;
                off_y[12] = init >> 5;
                init += height;
                off_y[13] = init >> 5;
                init += height;
                off_y[14] = init >> 5;
                init += height;
                off_y[15] = init >> 5;
                init += height;
                off_y[16] = init >> 5;
                init += height;
                off_y[17] = init >> 5;
                init += height;
                off_y[18] = init >> 5;
                init += height;
                off_y[19] = init >> 5;
                init += height;
                off_y[20] = init >> 5;
                init += height;
                off_y[21] = init >> 5;
                init += height;
                off_y[22] = init >> 5;
                init += height;
                off_y[23] = init >> 5;
                init += height;
                off_y[24] = init >> 5;
                init += height;
                off_y[25] = init >> 5;
                init += height;
                off_y[26] = init >> 5;
                init += height;
                off_y[27] = init >> 5;
                init += height;
                off_y[28] = init >> 5;
                init += height;
                off_y[29] = init >> 5;
                init += height;
                off_y[30] = init >> 5;
                init += height;
                off_y[31] = init >> 5;
            }

            for (int y = 0; y < 32; y++)
            {
                int offy = off_y[y];
                sized[0, y] = image2[off_x[0], offy] / 49F;
                sized[1, y] = image2[off_x[1], offy] / 49F;
                sized[2, y] = image2[off_x[2], offy] / 49F;
                sized[3, y] = image2[off_x[3], offy] / 49F;
                sized[4, y] = image2[off_x[4], offy] / 49F;
                sized[5, y] = image2[off_x[5], offy] / 49F;
                sized[6, y] = image2[off_x[6], offy] / 49F;
                sized[7, y] = image2[off_x[7], offy] / 49F;
                sized[8, y] = image2[off_x[8], offy] / 49F;
                sized[9, y] = image2[off_x[9], offy] / 49F;
                sized[10, y] = image2[off_x[10], offy] / 49F;
                sized[11, y] = image2[off_x[11], offy] / 49F;
                sized[12, y] = image2[off_x[12], offy] / 49F;
                sized[13, y] = image2[off_x[13], offy] / 49F;
                sized[14, y] = image2[off_x[14], offy] / 49F;
                sized[15, y] = image2[off_x[15], offy] / 49F;
                sized[16, y] = image2[off_x[16], offy] / 49F;
                sized[17, y] = image2[off_x[17], offy] / 49F;
                sized[18, y] = image2[off_x[18], offy] / 49F;
                sized[19, y] = image2[off_x[19], offy] / 49F;
                sized[20, y] = image2[off_x[20], offy] / 49F;
                sized[21, y] = image2[off_x[21], offy] / 49F;
                sized[22, y] = image2[off_x[22], offy] / 49F;
                sized[23, y] = image2[off_x[23], offy] / 49F;
                sized[24, y] = image2[off_x[24], offy] / 49F;
                sized[25, y] = image2[off_x[25], offy] / 49F;
                sized[26, y] = image2[off_x[26], offy] / 49F;
                sized[27, y] = image2[off_x[27], offy] / 49F;
                sized[28, y] = image2[off_x[28], offy] / 49F;
                sized[29, y] = image2[off_x[29], offy] / 49F;
                sized[30, y] = image2[off_x[30], offy] / 49F;
                sized[31, y] = image2[off_x[31], offy] / 49F;
            }

            //DCT
            float[,] res1 = new float[32, 32];
            float[,] res2 = new float[32, 32];
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    float val = DCT[0, y] * sized[x, 0];
                    val += DCT[1, y] * sized[x, 1];
                    val += DCT[2, y] * sized[x, 2];
                    val += DCT[3, y] * sized[x, 3];
                    val += DCT[4, y] * sized[x, 4];
                    val += DCT[5, y] * sized[x, 5];
                    val += DCT[6, y] * sized[x, 6];
                    val += DCT[7, y] * sized[x, 7];
                    val += DCT[8, y] * sized[x, 8];
                    val += DCT[9, y] * sized[x, 9];
                    val += DCT[10, y] * sized[x, 10];
                    val += DCT[11, y] * sized[x, 11];
                    val += DCT[12, y] * sized[x, 12];
                    val += DCT[13, y] * sized[x, 13];
                    val += DCT[14, y] * sized[x, 14];
                    val += DCT[15, y] * sized[x, 15];
                    val += DCT[16, y] * sized[x, 16];
                    val += DCT[17, y] * sized[x, 17];
                    val += DCT[18, y] * sized[x, 18];
                    val += DCT[19, y] * sized[x, 19];
                    val += DCT[20, y] * sized[x, 20];
                    val += DCT[21, y] * sized[x, 21];
                    val += DCT[22, y] * sized[x, 22];
                    val += DCT[23, y] * sized[x, 23];
                    val += DCT[24, y] * sized[x, 24];
                    val += DCT[25, y] * sized[x, 25];
                    val += DCT[26, y] * sized[x, 26];
                    val += DCT[27, y] * sized[x, 27];
                    val += DCT[28, y] * sized[x, 28];
                    val += DCT[29, y] * sized[x, 29];
                    val += DCT[30, y] * sized[x, 30];
                    val += DCT[31, y] * sized[x, 31];
                    res1[x, y] = val;
                }
            }

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    float val = res1[0, y] * DCT_Transpose[x, 0];
                    val += res1[1, y] * DCT_Transpose[x, 1];
                    val += res1[2, y] * DCT_Transpose[x, 2];
                    val += res1[3, y] * DCT_Transpose[x, 3];
                    val += res1[4, y] * DCT_Transpose[x, 4];
                    val += res1[5, y] * DCT_Transpose[x, 5];
                    val += res1[6, y] * DCT_Transpose[x, 6];
                    val += res1[7, y] * DCT_Transpose[x, 7];
                    val += res1[8, y] * DCT_Transpose[x, 8];
                    val += res1[9, y] * DCT_Transpose[x, 9];
                    val += res1[10, y] * DCT_Transpose[x, 10];
                    val += res1[11, y] * DCT_Transpose[x, 11];
                    val += res1[12, y] * DCT_Transpose[x, 12];
                    val += res1[13, y] * DCT_Transpose[x, 13];
                    val += res1[14, y] * DCT_Transpose[x, 14];
                    val += res1[15, y] * DCT_Transpose[x, 15];
                    val += res1[16, y] * DCT_Transpose[x, 16];
                    val += res1[17, y] * DCT_Transpose[x, 17];
                    val += res1[18, y] * DCT_Transpose[x, 18];
                    val += res1[19, y] * DCT_Transpose[x, 19];
                    val += res1[20, y] * DCT_Transpose[x, 20];
                    val += res1[21, y] * DCT_Transpose[x, 21];
                    val += res1[22, y] * DCT_Transpose[x, 22];
                    val += res1[23, y] * DCT_Transpose[x, 23];
                    val += res1[24, y] * DCT_Transpose[x, 24];
                    val += res1[25, y] * DCT_Transpose[x, 25];
                    val += res1[26, y] * DCT_Transpose[x, 26];
                    val += res1[27, y] * DCT_Transpose[x, 27];
                    val += res1[28, y] * DCT_Transpose[x, 28];
                    val += res1[29, y] * DCT_Transpose[x, 29];
                    val += res1[30, y] * DCT_Transpose[x, 30];
                    val += res1[31, y] * DCT_Transpose[x, 31];
                    res2[x, y] = val;
                }
            }

            float[] s1 = new float[256];
            int cnt = 0;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    s1[cnt] = res2[x, y];
                    cnt++;
                }
            }

            return GenerateHashes(s1);
        }
    }
}