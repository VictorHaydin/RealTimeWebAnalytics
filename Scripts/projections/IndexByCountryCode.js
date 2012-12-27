fromStream("Visits").when({
    "RealTimeWebAnalytics.Models.Visit": function (state, e) {
        var code = e.body.CountryCode ? e.body.CountryCode : (null);
        linkTo('Country-' + code, e);
    }
});
