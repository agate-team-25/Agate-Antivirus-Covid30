public class P3K : Items
{
    public override void PickUp()
    {
        PlayerController.instance.itemType = ItemType.P3K;
        PlayerController.instance.PickUpItemEffect();
        Destroy(gameObject);
    }

    public override string GetName()
    {
        return "P3K";
    }
}
