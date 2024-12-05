#ifndef HEADER_FUGGVENYEK_INCLUDED
    #define HEADER_FUGGVENYEK_INCLUDED
    #include <stdio.h>

    void konzolosMegoldas(char paramsorsz[], int paramlenght);

    void fileosMegoldas(char paramsorsz[], int paramlenght, char ** argv, int argc);

    void withNumbers(char *p, int numLines, int paramlenght);

    void withoutNumbers(char *p, int numLines, int paramlenght);

    void memCheck( char* ptr );

    void determineIfRight(int length, char ** argv);

#endif