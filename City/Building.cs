﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace City
{
    public class Building : Engine.Actor
    {
        /// <summary>
        /// Primitive solution to prevent spawning buildings on each other
        /// </summary>
        public Rectangle collision;

        /// <summary>
        /// is in workable state. for example does it has power of resourses to work
        /// </summary>
        bool activated;

        public Building(GameHandler game, Rectangle collision, Vector3 location, Vector3 rotation, double lifeTime) : base(game, location, rotation, lifeTime)
        {
            this.collision = collision;
        }

        public Building(GameHandler game, string Name, Rectangle collision, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, location, rotation, lifeTime)
        {
            this.collision = collision;
        }

        public virtual bool Activated { get => activated; set => activated = value; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }

    public class PowerConsumingBuilding : Building
    {
        /// <summary>
        /// Range where builng can be powered from source. Should be bigger that collision
        /// </summary>
        public Rectangle powerAcceptanceRange;

        public bool powered;

        Engine.Components.ImageDisplayComponent noPowerIcon;

        public override bool Activated { get => base.Activated && powered; set => base.Activated = value; }

        public PowerConsumingBuilding(GameHandler game, Rectangle collision, Rectangle powerAcceptanceRange, Vector3 location, Vector3 rotation, double lifeTime) : base(game, collision, location, rotation, lifeTime)
        {
            this.powerAcceptanceRange = powerAcceptanceRange;
            noPowerIcon = new Engine.Components.ImageDisplayComponent(game, this, "Textures/buildings/icon_nopower");
        }

        public PowerConsumingBuilding(GameHandler game, string Name, Rectangle collision, Rectangle powerAcceptanceRange, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, collision, location, rotation, lifeTime)
        {
            this.powerAcceptanceRange = powerAcceptanceRange;
            noPowerIcon = new Engine.Components.ImageDisplayComponent(game, this, "Textures/buildings/icon_nopower");
        }

        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(powered);
            base.Update(gameTime);
            noPowerIcon.Update(gameTime);
        }

        public override void Init()
        {
            base.Init();
            noPowerIcon.Init();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!powered)
            { noPowerIcon.Draw(spriteBatch); }
        }

    }
    public class PowerSourceBuilding : Building
    {
        /// <summary>
        /// Range where other builng can be powered. Should be bigger that collision
        /// </summary>
        public Rectangle powerGererationRange;

        

        public PowerSourceBuilding(GameHandler game, Rectangle collision, Rectangle powerGererationRange, Vector3 location, Vector3 rotation, double lifeTime) : base(game, collision, location, rotation, lifeTime)
        {
            this.powerGererationRange = powerGererationRange;
            
        }

        public PowerSourceBuilding(GameHandler game, string Name, Rectangle collision, Rectangle powerGererationRange, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, collision, location, rotation, lifeTime)
        {
            this.powerGererationRange = powerGererationRange;
           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PowerBuildings();
        }

        public override void Init()
        {
            base.Init();
        }

        protected virtual void PowerBuildings()
        {
            if (Game != null)
            {
                foreach (PowerConsumingBuilding building in Game.actors.OfType<PowerConsumingBuilding>())
                {

                    if (building.powerAcceptanceRange.Intersects(powerGererationRange))
                    {
                        building.powered = true;
                        //do something
                    }
                    else
                    {
                        building.powered = false;
                    }

                }
            }
            else
            {
                throw new NullReferenceException("Attempt to use null Game Handler");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Texture2D _texture;

            _texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });

            spriteBatch.Draw(_texture, powerGererationRange, Color.Red);

            _texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });

            spriteBatch.Draw(_texture, collision, Color.Green);

            foreach (var comp in components)
            {
                if (comp is Engine.Components.DrawableComponent)
                {
                    (comp as Engine.Components.DrawableComponent).Draw(spriteBatch);

                }
            }
        }
    }

    
}
