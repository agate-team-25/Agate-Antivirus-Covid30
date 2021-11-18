public class WeaponBox : Items
{
    public override void PickUp()
    {
        Destroy(gameObject);
    }
}
