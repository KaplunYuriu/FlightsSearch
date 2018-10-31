namespace Graph.Entities
{
    public class Edge<T>
    {
        public double Length { get; set; }
        public double Cost { get; set; }
        public AsyncNode<T> ConnectedNode { get; set; }
    }
}