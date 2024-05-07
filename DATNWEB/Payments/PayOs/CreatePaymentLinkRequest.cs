namespace DATNWEB.Payments.PayOs;

public record CreatePaymentLinkRequest(
string productName,
string description,
int price,
string returnUrl,
string cancelUrl
);
