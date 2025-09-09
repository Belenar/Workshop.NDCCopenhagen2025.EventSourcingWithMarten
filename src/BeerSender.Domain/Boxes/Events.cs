namespace BeerSender.Domain.Boxes.Events;

public record BoxCreated(BoxCapacity Capacity);

public record BoxCreatedWithContainerType(BoxCapacity Capacity, ContainerType ContainerType);

public enum ContainerType
{
    BelgianBottle,
    DutchCan
}

public record ShippingLabelAdded(ShippingLabel Label);

public record FailedToAddShippingLabel(FailedToAddShippingLabel.FailReason Reason)
{
    public enum FailReason
    {
        InvalidCarrier,
        InvalidShippingLabel
    }
}

public record BeerBottleAdded(BeerBottle Bottle);

public record FailedToAddBeerBottle(FailedToAddBeerBottle.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasFull
    }
}

public record BoxClosed;

public record FailedToCloseBox(FailedToCloseBox.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasEmpty
    }
}


public record BoxSent;

public record FailedToSendBox(FailedToSendBox.FailReason Reason)
{
    public enum FailReason
    {
        BoxWasNotClosed,
        BoxHadNoLabel
    }
}
