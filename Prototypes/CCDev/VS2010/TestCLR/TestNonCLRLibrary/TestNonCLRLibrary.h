

#ifdef __cplusplus
    extern "C" {
#endif

        #define TEST_STRING_PROPERTY_SIZE 100

        typedef struct _TEST_INPUT_DATA
        {
            int intIntProperty ;
            char szStringProperty[ TEST_STRING_PROPERTY_SIZE ] ;
        } TEST_INPUT_DATA ;

        #define TEST_DESCRIPTION_ITEM_COUNT 100
        #define TEST_DESCRIPTION_ITEM_SIZE 100

        typedef struct _TEST_OUTPUT_DATA
        {
            double dblResult ;
            char aszDescriptionData[ TEST_DESCRIPTION_ITEM_COUNT ][ TEST_DESCRIPTION_ITEM_SIZE ] ;
        } TEST_OUTPUT_DATA ;

        extern "C" _declspec( dllexport ) int TestPerformFlatCalculation( const TEST_INPUT_DATA *pinputData , TEST_OUTPUT_DATA *poutputData ) ;

#ifdef __cplusplus
    /* extern "C" */ }
#endif
