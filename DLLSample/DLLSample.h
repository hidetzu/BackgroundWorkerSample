#pragma once

#include <cstdint>
extern "C" typedef __declspec(dllimport) void(__stdcall *t_dllCallBack)(int ret);
extern "C" __declspec(dllimport) double add(double a, double b);
extern "C" __declspec(dllimport) void start_thread(t_dllCallBack callback);
