﻿// CombatMap.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SpaceRPG.Source.Visuals.Maps.Isometric;
using SpaceRPG.Source.Visuals;
using SpaceRPG.Source.Gameplay.Combat.Maps.Isometric;

namespace SpaceRPG.Source.Gameplay.Combat.Maps
{
    /// <summary>
    /// CombatMap stores map data relevant to combat, such as tiles that are impassable, height of tiles, and so on (stored as CombatTiles).
    /// </summary>
    public class CombatMap
    {
        public Image Image;
        public CombatTile[,] Grid;
        public HashSet<Point> MoveOverlays;
        public Rectangle MoveEffectRect;
        public Vector2 TileDimensions, GridDimensions;

        public delegate void SetCursor(Vector2 loc);
        [XmlIgnore]
        public SetCursor SetCursorPosition;

        public CombatMap()
        {
            Grid = new CombatTile[0, 0];
            TileDimensions = Vector2.Zero;
            Image = new Image();
            MoveEffectRect = new Rectangle();
            MoveOverlays = new HashSet<Point>();
        }

        public void LoadContent(IsometricMapInfo mapInfo, SetCursor setCursor)
        {
            SetCursorPosition = setCursor;

            this.TileDimensions = mapInfo.TileDimensions;
            this.GridDimensions = mapInfo.LayerDimensions;
            Grid = new CombatTile[(int)GridDimensions.X, (int)GridDimensions.Y];

            // Load the content for each combat tile and store it in the grid
            for (int y = 0; y < GridDimensions.Y; y++)
            {
                for (int x = 0; x < GridDimensions.X; x++)
                {
                    Grid[x, y] = new CombatTile();
                    Grid[x, y].LoadContent(mapInfo.Tiles[x, y]);
                }
            }

            // Laod the tilesheet
            Image.Path = "Gameplay/TileSheets/ground";
            Image.LoadContent();
            MoveEffectRect = new Rectangle(0, 0, 32, 32); // Hardcoded, remove later
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            foreach (CombatTile tile in Grid)
            {
                tile.Update(gameTime);
                if (tile.HasCursor)
                    SetCursorPosition(tile.SourceTile.Position);
            }
        }

        public void DrawMoveRange(SpriteBatch spriteBatch)
        {
            foreach (Point p in MoveOverlays)
            {
                Image.Position = new Vector2(p.X*32, p.Y*32);
                Image.SourceRect = MoveEffectRect;
                Image.Draw(spriteBatch);
            }
        }

        public void DisplayMoveRange(int range, Vector2 location)
        {
            for (int y = 0; y < range; y++)
            {
                for (int x = 0; x < range; x++)
                {
                    if (x + y < range)
                    {
                        MoveOverlays.Add(new Point((int)location.X + x, (int)location.Y + y));
                        MoveOverlays.Add(new Point((int)location.X - x, (int)location.Y - y));
                        MoveOverlays.Add(new Point((int)location.X + x, (int)location.Y - y));
                        MoveOverlays.Add(new Point((int)location.X - x, (int)location.Y + y));
                    }
                }
            }
        }
    }
}
