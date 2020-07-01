using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSimulation
{
    class Ant : GameObject
    {
        private Nest nest;
        private bool hasFood = false;

        public Ant(Nest nest)
        {
            this.nest = nest;
        }

        public override void UpdateOn(World world)
        {
            if (hasFood)
            {
                Color = Color.Red;
                ReleasePheromone(world);
                MoveToNest(world);
            }
            else
            {
                Color = Color.Blue;
                Wander(world);
                if (!CheckFood(world))
                {
                    CheckPheromone(world);
                }
            }
        }

        private void Wander(World world)
        {
            Forward(world.Random(1, 5));
            Turn(world.Random(-25, 25));
        }

        private bool CheckFood(World world)
        {
            IEnumerable<GameObject> food = FindNear (world, 15);
            if (food.Any())
            {
                GameObject f = food.Where(each => each.Position.Equals(Position)).FirstOrDefault();
                if (f != null)
                {
                    hasFood = true;
                    world.Remove(f);
                }
                else
                {
                    LookTo(food.First().Position);
                    Wander(world);
                }
                return true;
            }
            return false;
        }

        private void CheckPheromone(World world)
        {
            int[] test = new int[4];
            PointF? strongestPoint = null;
            double strongestIntensity = 0;
            IEnumerable<Pheromone> nearPheromones = FindNear(world, 10).OfType<Pheromone>();
            foreach (Pheromone p in nearPheromones.Where(p => p.Intensity > 5))
            {
                if (p.Intensity > strongestIntensity)
                {
                    strongestIntensity = p.Intensity;
                    strongestPoint = p.Position;
                }
            }

            if (strongestPoint.HasValue)
            {
                LookTo(strongestPoint.Value);
                Wander(world);
            }
        }

        private void ReleasePheromone(World world)
        {
            Pheromone.SpawnOn(world, Position, world.Dist(Position, world.Center) * 1.5);
        }

        private void MoveToNest(World world)
        {
            LookTo(nest.Position);
            Wander(world);
            if (Position.Equals(nest.Position))
            {
                hasFood = false;
            }
        }

        private GameObject[] FindNear (World world, float radius)
        {
            GameObject[] temp = world.GameObjectsNear(Position,int.Parse(radius.ToString()));
            temp.Where(i => i != null);
            return temp;
        }
    }
}
