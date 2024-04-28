enum Provider {
    Openmeteo = 0,
    Weatherapi = 1
}

export function providerToString(provider: Provider) {
    switch(provider) {
        case Provider.Openmeteo: return "Openmeteo";
        case Provider.Weatherapi: return "Weatherapi";
    }
}

export default Provider;