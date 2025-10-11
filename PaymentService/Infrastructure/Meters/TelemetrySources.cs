using System.Diagnostics;

namespace Infrastructure.Meters
{
    public static class TelemetrySources
    {
        public static readonly ActivitySource PaymentServicePublisher = new("PaymentService");
    }

}
