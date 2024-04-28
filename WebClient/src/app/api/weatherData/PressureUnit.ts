enum PressureUnit {
    MilliBar = 0
}

export function pressureToString(unit: PressureUnit) {
    switch(unit) {
        case PressureUnit.MilliBar: return "mb";
    }
}

export default PressureUnit;