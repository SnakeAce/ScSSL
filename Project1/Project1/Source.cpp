#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <time.h>

using namespace std;

unsigned long long sz = (1 << 29);
int * ind = new int[sz];
int32_t * val = new int32_t[sz];

int main()
{
	clock_t start, end;
	unsigned long long v;
	int count = 0, i;
	for (i = 0; i < sz; ++i)
		ind[i] = i, val[i] = rand();
	printf("Loaded %llu elements. Total time: %.4lf\n", sz << 1, double(clock()) / CLOCKS_PER_SEC);
	printf("Enter search element: ");
	scanf("%llu", &v);
	start = clock();
	for (i = 0; i < sz; ++i)
	{
		if (v ^ val[i])
			continue;
		++count;
	}
	end = clock();
	printf("Found %d elements for %.4lf seconds\n", count, (double(end) - start) / (CLOCKS_PER_SEC));
	delete ind;
	delete val;
	system("pause");
	return 0;
}