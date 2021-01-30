using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using Game;
public class LoadLevelDataManager : MonoBehaviour
{
    Encoding encoding;

// CSVから切り分けられた文字列型２次元配列データ
    private string[,] sdataArrays;

//読み込めたか確認の表示用の変数
    private int height = 0; //行数
    private int width = 0; //列数

    [SerializeField] private bool isDebug;

    public void Update()
    {
        if (isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickedLoadCsvFileButton();
            }
        }
    }

// CSVデータを文字列型２次元配列に変換する
// ファイルパス,変換される配列の値(参照渡し)
    private void readCSVData(string path, ref string[,] sdata)
    {
        // ストリームリーダーsrに読み込む
        StreamReader sr = new StreamReader(path);
        // ストリームリーダーをstringに変換
        string strStream = sr.ReadToEnd();

        //以下のFile.ReadAllTextを試してみましたが、UTF8ではないファイルの日本語の文字列は文字化けしました。
        //string strStream = File.ReadAllText(path, this.encoding);

        // StringSplitOptionを設定(要はカンマとカンマに何もなかったら格納しないことにする)
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        // 行に分ける
        string[] lines = strStream.Split(new char[] {'\r', '\n'}, option);

        // カンマ分けの準備(区分けする文字を設定する)
        char[] spliter = new char [1] {','};

        // 行数設定
        int h = lines.Length;
        // 列数設定
        int widthMax = 0;
        for (int i = 0; i < h; i++)
        {
            widthMax = Math.Max(widthMax, lines[i].Split(spliter, option).Length);
        }

        int w = widthMax;

        // 返り値の2次元配列の要素数を設定
        sdata = new string [h, w];

        // 行データを切り分けて,2次元配列へ変換する
        for (int i = 0; i < h; i++)
        {
            string[] splitedData = lines[i].Split(spliter, option);

            for (int j = 0; j < w; j++)
            {
                if (j >= splitedData.Length)
                {
                    sdata[i, j] = "";
                }
                else
                {
                    sdata[i, j] = splitedData[j];
                }
            }
        }

        // 確認表示用の変数(行数、列数)を格納する
        this.height = h; //行数
        this.width = w; //列数
    }

// ２次元配列の型を文字列型から整数値型へ変換する
    private void convert2DArrayType(ref string[,] sarrays, ref int[,] iarrays, int h, int w)
    {
        iarrays = new int[h, w];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                iarrays[i, j] = int.Parse(sarrays[i, j]);
            }
        }
    }

//確認表示用の関数
//引数：2次元配列データ,行数,列数
    private void ApplyLevelDatas(string[,] arrays, int height, int width)
    {
        LevelData levelData = new LevelData(width, height);
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                //行番号-列番号:データ値 と表示される
                Debug.Log(x + "-" + y + ":" + arrays[x, y]);

                bool blockIsFixed = false;
                if (arrays[x, y].Substring(0, 1)=="0")
                {
                    blockIsFixed = false;
                }
                else if (arrays[x, y].Substring(0, 1)=="1")
                {
                    blockIsFixed = true;
                }
                bool blockIsFreeze = false;
                if (arrays[x, y].Substring(0, 1)=="0")
                {
                    blockIsFreeze = false;
                }
                else if (arrays[x, y].Substring(0, 1)=="1")
                {
                    blockIsFreeze = true;
                }
                //bool blockIsFreeze = System.Convert.ToBoolean(arrays[x, y].Substring(1, 1));//←使い方が分かりませんでした…。
                char blockValue = (char)int.Parse(arrays[x, y].Substring(2,1));
                Block block = new Block(blockValue,blockIsFixed,blockIsFreeze);
                levelData.SetAt(x,y,block);
            }
        }
    }

    public void ClickedLoadCsvFileButton()
    {
        //this.encoding = Encoding.GetEncoding("utf-8");
        //データパスを設定
        //このデータパスは、Assetフォルダ以下の位置を書くので/で階層を区切り、CSVデータ名まで書かないと読み込んでくれない
        string path = OpenFile();
        if (path == null)
        {
            return;
        }

        //データを読み込む(引数：データパス)
        readCSVData(path, ref this.sdataArrays);
        //readCSVData(Application.dataPath + path, ref this.sdataArrays);
        //convert2DArrayType(ref this.sdataArrays, ref this.stageMapDatas, this.height, this.width);

        ApplyLevelDatas(this.sdataArrays, this.height, this.width);
    }

    public string OpenFile()
    {
        //パスの取得
        var path = EditorUtility.OpenFilePanel("Open csv", "", "CSV");
        if (string.IsNullOrEmpty(path))
            return path;
        Debug.Log(path);

        //読み込み
        var reader = new StreamReader(path);
        Debug.Log(reader);
        return path;
    }
    public void SaveFile()
    {
        // //パスの取得
        // var path = EditorUtility.OpenFilePanel("Open csv", "", "CSV");
        // if (string.IsNullOrEmpty(path))
        //     return path;
        // Debug.Log(path);
        //
        // //読み込み
        // var reader = new StreamReader(path);
        // Debug.Log(reader);
        // return path;
        return;
    }
}
