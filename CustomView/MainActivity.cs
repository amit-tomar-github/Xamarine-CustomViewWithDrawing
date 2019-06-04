using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Views;
using Android.Content;
using System;

namespace CustomView
{
    [Activity(Label = "CustomView", MainLauncher = true)]
    public class MainActivity : Activity
    {
        DrawingView dv;
        public Paint mPaint;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            dv = new DrawingView(this);
            SetContentView(dv);
            mPaint = new Paint();
            mPaint.AntiAlias = true;
            mPaint.Dither = true;
            mPaint.Color = Color.Green;
            mPaint.SetStyle(Paint.Style.Stroke);
            mPaint.StrokeJoin = Paint.Join.Round;
            mPaint.StrokeCap = Paint.Cap.Round;
            mPaint.StrokeWidth = 12;

        }

        public class DrawingView : View
        {
            public int width;
            public int height;
            private Bitmap mBitmap;
            private Canvas mCanvas;
            private Path mPath;
            private Paint mBitmapPaint;
            Context context;
            private Paint circlePaint;
            private Path circlePath;
            public DrawingView(Context context) : base(context)
            {
                this.context = context;
                mPath = new Path();
                mBitmapPaint = new Paint(PaintFlags.Dither);
                circlePaint = new Paint();
                circlePath = new Path();

                circlePaint.AntiAlias = true;
                circlePaint.Color = Color.Blue;
                circlePaint.SetStyle(Paint.Style.Stroke);
                circlePaint.StrokeJoin = Paint.Join.Miter;
                circlePaint.StrokeWidth = 4f;

            }
            protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
            {
                base.OnSizeChanged(w, h, oldw, oldh);

                mBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
                mCanvas = new Canvas(mBitmap);
            }
            protected override void OnDraw(Canvas canvas)
            {
                base.OnDraw(canvas);

                canvas.DrawBitmap(mBitmap, 0, 0, mBitmapPaint);
                // canvas.DrawPath(mPath, mPaint);
                canvas.DrawPath(mPath, circlePaint);
                canvas.DrawPath(circlePath, circlePaint);
            }

            private float mX, mY;
            private static float TOUCH_TOLERANCE = 4;

            private void touch_start(float x, float y)
            {
                mPath.Reset();
                mPath.MoveTo(x, y);
                mX = x;
                mY = y;
            }

            private void touch_move(float x, float y)
            {
                float dx = Math.Abs(x - mX);
                float dy = Math.Abs(y - mY);
                if (dx >= TOUCH_TOLERANCE || dy >= TOUCH_TOLERANCE)
                {
                    mPath.QuadTo(mX, mY, (x + mX) / 2, (y + mY) / 2);
                    mX = x;
                    mY = y;

                    circlePath.Reset();
                    circlePath.AddCircle(mX, mY, 30, Path.Direction.Cw);
                }
            }

            private void touch_up()
            {
                mPath.LineTo(mX, mY);
                circlePath.Reset();
                // commit the path to our offscreen
                //  mCanvas.DrawPath(mPath, mPaint);
                mCanvas.DrawPath(mPath, circlePaint);
                // kill this so we don't double draw
                mPath.Reset();
            }
            public override bool OnTouchEvent(MotionEvent e)
            {
                float x = e.GetX();
                float y = e.GetY();
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        touch_start(x, y);
                        Invalidate();
                        break;
                    case MotionEventActions.Move:
                        touch_move(x, y);
                        Invalidate();
                        break;
                    case MotionEventActions.Up:
                        touch_up();
                        Invalidate();
                        break;
                }
                return true;
            }
            //public bool OnTouch(View v, MotionEvent e)
            //{
            //    float x = e.GetX();
            //float y = e.GetY();
            //    switch (e.Action)
            //    {
            //        case MotionEventActions.Down:
            //            touch_start(x, y);
            //            Invalidate();
            //            break;
            //        case MotionEventActions.Move:
            //            touch_move(x, y);
            //            Invalidate();
            //            break;
            //        case MotionEventActions.Up:
            //            touch_up();
            //            Invalidate();
            //            break;
            //    }
            //    return true;
            //}
        }
    }
}

