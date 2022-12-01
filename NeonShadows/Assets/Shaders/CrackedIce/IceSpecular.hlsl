void IceSpecular_half(half3 Specular, half3 lightDir, half Smoothness, half3 Color, half3 WorldNormal, half3 WorldView, out half3 Out) {
    #if SHADERGRAPH_PREVIEW
        Out = 1;
    #else
        // get light direction
        Smoothness = exp2(10 * Smoothness + 1);
        WorldNormal = normalize(WorldNormal);
        WorldView = SafeNormalize(WorldView);
        Out = LightingSpecular(Color, normalize(lightDir), WorldNormal, WorldView, half4(Specular, 0), Smoothness);
    #endif
}