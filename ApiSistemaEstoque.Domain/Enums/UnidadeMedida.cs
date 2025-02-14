namespace ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums
{
    public enum UnidadeMedida
    {
        // Massa
        Miligrama,      // mg
        Grama,          // g
        Quilograma,     // kg
        Tonelada,       // t

        // Volume
        Mililitro,      // mL
        Centilitro,     // cL
        Decilitro,      // dL
        Litro,          // L
        CentimetroCubico, // cm³
        MilimetroCubico, // mm³
        MetroCubico,    // m³

        // Comprimento
        Milimetro,      // mm
        Centimetro,     // cm
        Metro,          // m
        Quilometro,     // km
        Polegada,       // in
        Pé,             // ft
        Jarda,          // yd

        // Área
        MilimetroQuadrado, // mm²
        CentimetroQuadrado, // cm²
        MetroQuadrado,   // m²
        Hectare,         // ha
        Acre,            // ac

        // Tempo
        Segundo,        // s
        Minuto,         // min
        Hora,           // h
        Dia,            // d
        Semana,         // semana
        Mes,            // mês
        Ano,            // ano

        // Quantidade
        Unidade,        // un (unidade simples)
        Par,            // par de itens
        Duzia,          // 12 unidades
        Pacote,         // pacote de itens
        Caixa,          // caixa de itens
        Centena,        // 100 unidades
        Milhar,         // 1000 unidades

        // Eletricidade
        Ampere,         // A
        Volt,           // V
        Watt,           // W
        Quilowatt,      // kW
        Megawatt,       // MW

        // Outros
        Caloria,        // cal
        Quilocaloria,   // kcal
        Joule,          // J
        Quilojoule,     // kJ
    }
}
