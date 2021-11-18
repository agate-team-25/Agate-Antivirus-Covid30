public class APDBox : Items
{
    public override void PickUp()
    {
        Destroy(gameObject);
    }
}
