﻿// GameObject.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceRPG
{
    /// <summary>
    /// GameObject is the base object for all game objects in the world
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// Loads content
        /// </summary>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Unloads content
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Updates based on gametime
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the current spritebatch to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
