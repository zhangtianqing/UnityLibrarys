﻿using System;
using UnityEngine;

namespace Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts
{
    public class ByteArray
    {
        //默认大小
        const int DEFAULT_SIZE = 1024;
        //初始大小
        int initSize = 0;
        //缓冲区
        public byte[] bytes;
        //读写位置
        public int startIndex
        {
            get
            {
                //Debug.LogError("StartIUndexDebug：" + StartIUndexDebug);
                return StartIUndexDebug;
            }
            set { StartIUndexDebug = value; }
        }
        private int StartIUndexDebug = 0;
        private int capacity = 0;

        public int writeIdx = 0;
        //容量
        //剩余空间
        public int remain { get { return capacity - writeIdx; } }
        //数据长度
        public int length { get { return writeIdx - startIndex; } }

        //构造函数
        public ByteArray(int size = DEFAULT_SIZE)
        {
            bytes = new byte[size];
            capacity = size;
            initSize = size;
            startIndex = 0;
            writeIdx = 0;
        }

        //构造函数
        public ByteArray(byte[] defaultBytes)
        {
            bytes = defaultBytes;
            capacity = defaultBytes.Length;
            initSize = defaultBytes.Length;
            startIndex = 0;
            writeIdx = defaultBytes.Length;
        }

        //重设尺寸
        public void ReSize(int size)
        {
            if (size < length) return;
            if (size < initSize) return;
            int n = 1;
            while (n < size) n *= 2;
            capacity = n;
            byte[] newBytes = new byte[capacity];
            Array.Copy(bytes, startIndex, newBytes, 0, writeIdx - startIndex);
            bytes = newBytes;
            writeIdx = length;
            startIndex = 0;
        }

        //写入数据
        public int Write(byte[] bs, int offset, int count)
        {
            if (remain < count)
            {
                ReSize(length + count);
            }
            Array.Copy(bs, offset, bytes, writeIdx, count);
            writeIdx += count;
            return count;
        }

        //读取数据
        public int Read(byte[] bs, int offset, int count)
        {
            count = Math.Min(count, length);
            Array.Copy(bytes, 0, bs, offset, count);
            startIndex += count;
            CheckAndMoveBytes();
            return count;
        }

        //检查并移动数据
        public void CheckAndMoveBytes()
        {
            if (length < 8)
            {
                MoveBytes();
            }
        }

        //移动数据
        public void MoveBytes()
        {
            Array.Copy(bytes, startIndex, bytes, 0, length);
            writeIdx = length;
            startIndex = 0;
        }

        //打印缓冲区
        public override string ToString()
        {
            return BitConverter.ToString(bytes, startIndex, length);
        }

        //打印调试信息
        public string DebugStr()
        {
            return string.Format("readIdx({0}) writeIdx({1}) bytes({2})",
                startIndex,
                writeIdx,
                BitConverter.ToString(bytes, 0, capacity)
            );
        }
    }
}