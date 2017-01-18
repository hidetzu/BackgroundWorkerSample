#include "DLLSample.h"
#include <Windows.h>
#include <thread>

double add(double a, double b)
{
	return a + b;
}

DWORD WINAPI ThreadProc(LPVOID pData) {
	t_dllCallBack callback = (t_dllCallBack)pData;
	std::this_thread::sleep_for(std::chrono::seconds(10));

	callback(0);

	return 0;
}

void start_thread(t_dllCallBack callback)
{
	DWORD DllThreadID;
	HANDLE DllThread; //thread's handle

	DllThread = CreateThread(NULL, 0, ThreadProc, callback, 0, &DllThreadID);
	if (DllThread == NULL)
		MessageBox(NULL, L"Error", L"Error", MB_OK);
	else
		CloseHandle(DllThread);

}