[System.Serializable]
public class CustomerPC
{
    public string cpu;

    public int ramSize;

    public string gpu;

    public CustomerPC Clone()
    {
        CustomerPC clone =
            new CustomerPC();

        clone.cpu = cpu;
        clone.ramSize = ramSize;
        clone.gpu = gpu;

        return clone;
    }
}