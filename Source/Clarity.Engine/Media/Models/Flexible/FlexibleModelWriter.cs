using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Clarity.Engine.Media.Models.Flexible
{
    public class FlexibleModelWriter
    {
        public static void WriteModel(TextWriter textWriter, IFlexibleModel model)
        {
            using (var writer = new JsonTextWriter(textWriter))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                {
                    writer.WritePropertyName("VertexSets");
                    writer.WriteStartArray();
                    foreach (var vertexSet in model.VertexSets)
                    {
                        writer.WriteStartObject();
                        {
                            writer.WritePropertyName("ArraySubranges");
                            writer.WriteStartArray();
                            {
                                foreach (var arraySubrange in vertexSet.ArraySubranges)
                                {
                                    writer.WriteStartObject();
                                    {
                                        var bytes = new byte[arraySubrange.Length];
                                        var ptr = arraySubrange.RawDataResource.Map();
                                        Marshal.Copy(ptr + arraySubrange.StartOffset, bytes, 0, arraySubrange.Length);
                                        arraySubrange.RawDataResource.Unmap(false);
                                        writer.WritePropertyName("Data");
                                        writer.WriteValue(bytes);
                                    }
                                    writer.WriteEnd();
                                }
                            }
                            writer.WriteEnd();

                            var indicesInfo = vertexSet.IndicesInfo;
                            writer.WritePropertyName("IndicesInfo");
                            if (indicesInfo == null)
                                writer.WriteNull();
                            else
                            {
                                writer.WriteStartObject();
                                {
                                    writer.WritePropertyName("ArrayIndex");
                                    writer.WriteValue(indicesInfo.ArrayIndex);
                                    writer.WritePropertyName("Format");
                                    writer.WriteValue(indicesInfo.Format.ToString());
                                }
                                writer.WriteEnd();
                            }

                            writer.WritePropertyName("ElementInfos");
                            writer.WriteStartArray();
                            {
                                foreach (var elementInfo in vertexSet.ElementInfos)
                                {
                                    writer.WriteStartObject();
                                    {
                                        writer.WritePropertyName("ArrayIndex");
                                        writer.WriteValue(elementInfo.ArrayIndex);
                                        writer.WritePropertyName("Format");
                                        writer.WriteValue(elementInfo.Format.ToString());
                                        writer.WritePropertyName("Stride");
                                        writer.WriteValue(elementInfo.Stride);
                                        writer.WritePropertyName("Offset");
                                        writer.WriteValue(elementInfo.Offset);
                                        writer.WritePropertyName("CommonSemantic");
                                        writer.WriteValue(elementInfo.CommonSemantic.ToString());
                                        writer.WritePropertyName("Semantic");
                                        writer.WriteValue(elementInfo.Semantic);
                                    }
                                    writer.WriteEnd();
                                }
                            }
                            writer.WriteEnd();
                        }
                        writer.WriteEnd();
                    }
                    writer.WriteEnd();

                    writer.WritePropertyName("Parts");
                    writer.WriteStartArray();
                    foreach (var part in model.Parts)
                    {
                        writer.WriteStartObject();
                        {
                            writer.WritePropertyName("VertexSetIndex");
                            writer.WriteValue(part.VertexSetIndex);
                            writer.WritePropertyName("PrimitiveTopology");
                            writer.WriteValue(part.PrimitiveTopology.ToString());
                            writer.WritePropertyName("VertexOffset");
                            writer.WriteValue(part.VertexOffset);
                            writer.WritePropertyName("FirstIndex");
                            writer.WriteValue(part.FirstIndex);
                            writer.WritePropertyName("IndexCount");
                            writer.WriteValue(part.IndexCount);
                            writer.WritePropertyName("ModelMaterialName");
                            writer.WriteValue(part.ModelMaterialName);
                        }
                        writer.WriteEnd();
                    }
                    writer.WriteEnd();

                    writer.WritePropertyName("Radius");
                    writer.WriteValue(model.Radius);
                }
                writer.WriteEnd();
            }
        }
    }
}