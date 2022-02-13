namespace Boids
{
    public class BoidSettings
    {
        public float minSpeed = 2;
        public float maxSpeed = 3;
        public float maxSteerForce = 0.7f;

        public float alignmentWeight = 0.1f;
        public float cohesionWeight = 0.1f;
        public float seperationWeight = 1.2f;
        public float obstacleWeight = 1;
        public float momentumWeight = 0.3f;
        public float distractionWeight = 0.1f;

        public float alignmentRadius = 100;
        public float cohesionRadius = 70;
        public float seperationRadius = 20;
        public float obstacleRadius = 100;

        public float targetWeight = 1;
        public float lineMulti = 32;
    }
}
