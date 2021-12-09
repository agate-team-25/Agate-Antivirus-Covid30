public class APDBox : Items
{
    public override void PickUp()
    {
        PlayerController.instance.itemType = ItemType.APDBox;
        PlayerController.instance.PickUpItemEffect();
        Destroy(gameObject);
    }
}
