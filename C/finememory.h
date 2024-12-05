#ifndef HEADER_FINEMEMORY_INCLUDED
    #define HEADER_FINEMEMORY_INCLUDED
    #include <stdio.h>
    #include <stdlib.h>

    void memCheck( char* ptr )
    {
        if (ptr == NULL)
        {
            printf("\nMemory allocation failed!\n");
            exit(99);
        }
    }


#endif