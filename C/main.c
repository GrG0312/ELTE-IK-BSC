#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "fuggvenyek.h"
struct parameterek
{
  char *sorsz;
  int maxkarakter;
};

int main(int argc, char ** argv) 
{

  determineIfRight(argc, argv);
  //printf("%s\n", argv[2]);
  struct parameterek arguments = { .maxkarakter = atoi(argv[2]), .sorsz = argv[1]};

  if (argc == 3)
  {
    konzolosMegoldas(arguments.sorsz, arguments.maxkarakter);
  } else
  {
    fileosMegoldas(arguments.sorsz, arguments.maxkarakter, argv, argc);
  }

  return 0;
}