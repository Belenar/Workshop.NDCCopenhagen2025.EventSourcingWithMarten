using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Commands;

namespace BeerSender.Domain.Tests.Boxes;

public class AddShippingLabelTest(MartenFixture fixture) 
    : BoxTest<AddShippingLabel>(fixture)
{
    protected override ICommandHandler<AddShippingLabel> Handler
        => new AddLabelHandler();
    
    [Theory]
    [InlineData(Carrier.UPS, "ABC123")]
    [InlineData(Carrier.UPS, "ABC999")]
    [InlineData(Carrier.FedEx, "DEF123")]
    [InlineData(Carrier.FedEx, "DEF999")]
    [InlineData(Carrier.PostDanmark, "GHI123")]
    [InlineData(Carrier.PostDanmark, "GHI999")]
    public async Task WhenUsingValidLabel_ShouldAddLabel(
        Carrier carrier, string tracking_code)
    {
        await Given<Box>(
            Box_created_with_capacity(24)
        );
        await When(
            Add_label_with_carrier_and_code(carrier, tracking_code)
        );
        await Then(
            Shipping_label_added_with_carrier_and_code(carrier, tracking_code)
        );
    }

    [Theory]
    [InlineData(Carrier.UPS, "AB123")]
    [InlineData(Carrier.UPS, "ZZZ999")]
    [InlineData(Carrier.FedEx, "DE123")]
    [InlineData(Carrier.FedEx, "ZZZ999")]
    [InlineData(Carrier.PostDanmark, "GH123")]
    [InlineData(Carrier.PostDanmark, "ZZZ999")]
    public async Task WhenUsingInvalidLabel_ShouldFail(
        Carrier carrier, string tracking_code)
    {
        await Given<Box>(
            Box_created_with_capacity(24)
        );

        await When(
            Add_label_with_carrier_and_code(carrier, tracking_code)
        );

        await Then(
            Tracking_code_was_invalid()
        );
    }

    protected AddShippingLabel Add_label_with_carrier_and_code(Carrier carrier, string trackingCode)
    {
        return new AddShippingLabel(Box_Id, new ShippingLabel(carrier, trackingCode));
    }
}