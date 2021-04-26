using System;

namespace rt
{
    public class Sphere : Geometry
    {
        private Vector Center { get; set; }
        private double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }


        public override Intersection GetIntersection(Line line,double minDist,double maxDist){
            //difference that we use often
            Vector diff = line.X0 - Center;
            //equation of level 2 data
            double a = line.Dx*line.Dx;
            double b = (diff*line.Dx)*2.0;
            double c = diff*diff - Radius*Radius;
            double delta = b*b - 4*a*c;
            double t1 = -b - Math.Sqrt(delta);
            double t2 = -b + Math.Sqrt(delta);
            double t = -1e9;
            //if delta<0 -> nothing
            if(delta<0){
                return new Intersection();
            }
            if(t1 > 0.0){
                t = t1/(2.0*a);
            }
            else if(t2 > 0.0){
                t = t2/(2.0*a);
            }
            if(t == -1e9){
                return new Intersection();
            }
            // returning the intersection when everything is fine 
            if(t>minDist && t<maxDist){
                return new Intersection(true,true,this,line,t);
            }
            return new Intersection();
        }

        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}