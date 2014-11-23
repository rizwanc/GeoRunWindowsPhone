using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeoRunWindowsPhone
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int totalNumPart;
        private int ttl;

        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            totalNumPart = 1;
        }

        // partNum is the number of particles being created ttlAddNum is the number that gets randomly added to give it life
        public void Update(int partNum, int ttlAddNum, int velH)
        {
            totalNumPart = partNum;
            for (int i = 0; i < totalNumPart; i++)
            {
                particles.Add(GenerateNewParticle(ttlAddNum, velH));
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle(int ttlAddNum, int velH)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            position.Y = position.Y + 15;
            position.Y += random.Next(30);
            Vector2 velocity = new Vector2(
                                    -1f * (float)(random.NextDouble() * velH),
                                    random.Next(-1, 2) * (float)(random.NextDouble() * velH));
            float angle = 90;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = .4f;
            ttl = 20 + random.Next(ttlAddNum);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}