#ifndef HEADER_RIGHT_INCLUDED
    #define HEADER_RIGHT_INCLUDED
    #include <stdio.h>
    #include <string.h>

    int determineIfRight(char ln[], char maxlength[], int length)
    {
        if (strcmp(ln, "linenums") != 0 && strcmp(ln, "nolinenums") != 0)
        {
            printf("Usage:\n\trev [SHOW LINE NUMBERS] [MAX LINE LENGTH] files...");
            return 1;
        }
        for (int i = 0; maxlength[i] != '\0'; ++i)
        {
            if(maxlength[i] < 48 || maxlength[i] > 57)  //ASCII kódos történet  
            {
                printf("Usage:\n\trev [SHOW LINE NUMBERS] [MAX LINE LENGTH] files...");
                return 1;
            }
        }
        return 0;
    }


#endif