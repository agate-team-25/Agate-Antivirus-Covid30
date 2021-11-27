public class WeaponBox : Items
{
    public override void PickUp()
    {
        PlayerController.instance.itemType = ItemType.WeaponBox;
        PlayerController.instance.PickUpItemEffect();
        Destroy(gameObject);
    }
}
