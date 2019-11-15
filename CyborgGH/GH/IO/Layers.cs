using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.GH.IO
{
    public static class Layers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_layerPath"></param>
        public static void CreateNestedLayer(string i_layerPath)
        {

            //break up input string
            string[] separators = { "::" };
            string[] layers = i_layerPath.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            int layerIndex = Rhino.RhinoDoc.ActiveDoc.Layers.FindByFullPath(layers[layers.Length - 1], -1);

            if (layerIndex < 0)
            {
                //create layers recursively
                var docLayers = RhinoDoc.ActiveDoc.Layers;
                for (int j = 0; j < layers.Length; j++)
                {
                    int layerInd = docLayers.FindByFullPath(layers[j], -1);
                    if (layerInd < 0)
                    {
                        if (j == 0)
                        {
                            layerInd = docLayers.Add(layers[0], System.Drawing.Color.Orange);
                        }
                        else
                        {
                            var tempLayer = new Rhino.DocObjects.Layer();
                            tempLayer.ParentLayerId = docLayers.FindName(layers[j - 1]).Id;
                            tempLayer.Name = layers[j];

                            layerInd = docLayers.Add(tempLayer);
                        }
                    }
                }
            }

            var lay = Rhino.RhinoDoc.ActiveDoc.Layers.FindName(layers[layers.Length - 1]);
            var attr = new Rhino.DocObjects.ObjectAttributes();
            attr.LayerIndex = lay.Index;
        }
    }
}
