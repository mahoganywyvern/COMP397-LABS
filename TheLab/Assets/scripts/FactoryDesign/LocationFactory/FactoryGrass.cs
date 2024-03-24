public class FactoryGrass : AbstractLocationFactory
{
    public override ILocation CreateLocation()
    {
        return new LocationGrass();
    }
}