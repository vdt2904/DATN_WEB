namespace DATNWEB.Payments.PayOs;

public record Response(
int error,
String message,
object? data
);
