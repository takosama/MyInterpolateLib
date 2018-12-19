using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp83
{
    class Program
    {
        static void Main(string[] args)
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(100, 100);

            var liner = new Interpolate.LinerInterPolate(100, 200, startPosition, endPosition);

            //時刻90から210までシュミレーション　時間100から200まで移動 開始位置(0,0) 終了位置(100,100)
            Enumerable.Range(90, 120).Select(x => liner.GetPosition(x)).ForEach(x => x.View());
        }
    }

    public static class MyLinqEx
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (var n in input)
                action(n);
            return input;
        }
    }

    public class Position
    {
        public float x { get; }
        public float y { get; }
        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void View()
        {
            Console.WriteLine("x:" + x + " y:" + y);
        }

        public static Position operator +(Position p0, Position p1) => new Position(p0.x + p1.x, p0.y + p1.y);
        public static Position operator -(Position p0, Position p1) => new Position(p0.x - p1.x, p0.y - p1.y);

        public static Position operator *(float p, Position pos) => new Position(pos.x * p, pos.y * p);
        public static Position operator *(Position pos, float p) => new Position(pos.x * p, pos.y * p);
    }

   

    public class LinerInterpolateThings : IInterpolateThings
    {
        Position startPosition;
        Position endPosition;
        public LinerInterpolateThings(float startTime, float endTime, Position startPosition, Position endPosition)
        {
            this.start = startTime;
            this.end = endTime;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
        public float start { private set; get; }

        public float end { private set; get; }

        public Position Function(float time)
        {
            return startPosition + (endPosition - startPosition) * time;
        }
    }


    public interface IInterpolateThings
    {

        float start { get; }
        float end { get; }
        Position Function(float time);
    }

    public class Interpolate
    {
        public class LinerInterPolate : Interpolate
        {
            public LinerInterPolate(float startTime, float endTime, Position startPosition, Position endPosition) : base()
            {
                base.Init(new LinerInterpolateThings(startTime, endTime, startPosition, endPosition));
            }
        }

        IInterpolateThings interpolate;
        public Interpolate(IInterpolateThings interpolateThings)
        {
            Init(interpolateThings);
        }
        public Interpolate()
        {

        }
        protected void Init(IInterpolateThings interpolateThings)
        {
            this.interpolate = interpolateThings;
        }
        float ConvertTimeScale(float time)
        {
            if (time < interpolate.start) return 0;
            if (time > interpolate.end) return 1;
            return (time - interpolate.start) / (interpolate.end - interpolate.start);
        }
        public Position GetPosition(float Time) => interpolate.Function(ConvertTimeScale(Time));
    }
}
