namespace BeerSender.Domain.Boxes.Events;

public record BoxCreated(BoxCapacity Capacity);

public record ShippingLabelAdded(ShippingLabel Label);

public record FailedToAddShippingLabel(FailedToAddShippingLabel.FailReason Reason)
{
    public enum FailReason
    {
        InvalidCarrier,
        InvalidShippingLabel
    }
}
