using System;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light)
        {
            // ADD CODE HERE: Detect whether the given point has a clear line of sight to the given light
            return true;
        }

        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var viewParallel = (camera.Up ^ camera.Direction).Normalize();

            var image = new Image(width, height);

            var vecW = camera.Direction * camera.ViewPlaneDistance;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var offsetHeight = ImageToViewPlane(j,height,camera.ViewPlaneHeight);
                    var offsetWidth = ImageToViewPlane(i,width,camera.ViewPlaneWidth);
                    var x1 = camera.Position + vecW + camera.Up*offsetHeight + viewParallel*offsetWidth;
                    var ray = new Line(camera.Position,x1);
                    Intersection intersection = FindFirstIntersection(ray,camera.FrontPlaneDistance,camera.BackPlaneDistance);
                    Color color = new Color();
                    if(intersection.Valid){
                        foreach(var light in lights){
                            var L = light.Position;
                            var C = camera.Position;
                            var V = intersection.Position;
                            var E = C - V;
                            E.Normalize();
                            var N = intersection.Geometry.Normal(V);
                            N.Normalize();
                            var T = L - V;
                            T.Normalize();
                            var R = N * ( N * T ) * 2 - T ;
                            R.Normalize();
                            Material material = intersection.Geometry.Material;
                            color = material.Ambient * light.Ambient;
                            if(N * T > 0 ){
                                color += material.Diffuse * light.Diffuse * ( N * T );
                            }
                            if(E * R > 0 ){
                                color += material.Specular * light.Specular * Math.Pow(E * R, material.Shininess);
                            }
                            color *= light.Intensity;
                        }
                        image.SetPixel(i, j, color);
                    }
                    else{
                        image.SetPixel(i,j,color);
                    }
                    // ADD CODE HERE: Implement pixel color calculation
                }
            }

            image.Store(filename);
        }
    }
}