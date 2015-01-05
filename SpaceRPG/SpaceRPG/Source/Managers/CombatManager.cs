﻿// CombatManager.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SpaceRPG.Source.Screens;
using SpaceRPG.Source.Visuals.Maps;
using SpaceRPG.Source.Gameplay.Combat;
using SpaceRPG.Source.Gameplay.Combat.Actors;
using SpaceRPG.Source.Gameplay.Combat.Maps;

namespace SpaceRPG.Source.Managers
{
    public class CombatManager
    {
        private Party _party;
        private Encounter _encounter;
        private CombatMap _combatMap;
        private int _currentTurn;
        private Agent _currentAgent;

        public CombatManager()
        {
            _party = new Party();
            _encounter = new Encounter();
            _combatMap = new CombatMap();
            _currentAgent = new Agent();
        }

        public void LoadContent(Map _map)
        {
            // Load the party
            XmlManager<Party> partyLoader = new XmlManager<Party>();
            _party = partyLoader.Load("Load/Gameplay/Combat/Party.xml");
            _party.LoadContent();
            // This is a poor way to do this, fix later
            foreach (Agent a in _party.Members)
            {
                a.TurnIsOver = ChangeTurns;
            }
                

            // Load the encounter
            XmlManager<Encounter> encounterLoader = new XmlManager<Encounter>();
            _encounter = encounterLoader.Load(CombatScreen.EncounterId +".xml");
            _encounter.LoadContent();

            // Load the combat map from the map combat layer
            _combatMap.LoadContent(_map.CombatLayer);
            
            // Start combat
            _currentTurn = _party.Members.Count-1;
            ChangeTurns();
        }

        public void UnloadContent()
        {
            _party.UnloadContent();
            _encounter.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            _party.Update(gameTime);
            _encounter.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_currentAgent.DisplayMoveRange)
                _combatMap.DrawMoveRange(spriteBatch);
            _encounter.Draw(spriteBatch);
            _party.Draw(spriteBatch);
        }

        public void ChangeTurns()
        {
            _currentAgent.MyTurn = false;
            _currentTurn++;
            if(_currentTurn >= _party.Members.Count)
                _currentTurn = 0;
            _currentAgent = _party.Members[_currentTurn];
            _currentAgent.MyTurn = true;
            _combatMap.MoveOverlays.Clear();
            _combatMap.DisplayMoveRange(_currentAgent.MoveRange, _currentAgent.Location);
        }
    }
}