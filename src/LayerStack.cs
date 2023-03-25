using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tetris
{
    public class LayerStack
    {
        private Collection<Layer> m_layers;

        public LayerStack()
        {
            m_layers = new Collection<Layer>();
        }

        public void PushLayer(Layer layer)
        {
            layer.Initialize();
            m_layers.Add(layer);
        }

        public void Update(GameTime time)
        {
            foreach (Layer layer in m_layers)
            {
                layer.Update(time);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in m_layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        //TODO handle layer deletion, LayerContent Unloading

    }
}