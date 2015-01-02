Unity_UI_Samples
================

幾つかのサンプル置き場（予定）

##無限スクロール
InfiniteScrollが該当します。  
![infinite scroll view](https://raw.github.com/wiki/tsubaki/Unity_UI_Samples/img/infinite scroll view.gif)

###使い方
InfiniteScrollコンポーネントでスクロール時の挙動を制御します。  
IInfiniteScrollSetupインターフェースを継承したコンポーネントから操作を受け付けます。  

1.  スクロールビューを作ります。(http://tsubakit1.hateblo.jp/entry/2014/12/18/040252)  
但しVerticalLayoutGroupやLayoutElementは使わず、手動で並べます。
2.  InfiniteScrollをScrollViewのContentに追加します。  
ScrollRectが親になるようにContentは設定する必要があります。
2.  InfiniteScrollのItem Baseに量産したいUI、InfiniteItemCountに作成するアイテム数を指定します。

###シーン

*  Infinite  
無限スクロールします。  
スクロールしたらスクロールした分だけ値が変化します。
*  Limited  
範囲を限定したスクロールです。  
例えば20個しか表現できない範囲のスクロールバーに1000個のアイテムがあった場合に、24個位のアイテム数を回して表現する事で初期化コストを抑えます。
*  Loop  
アイテムをループさせます。

###ポイント

*  scroll contentnのanchorやpivotはスクロールバーのスタート地点に設定します。
*  アイテムのanchorやpivotはscroll contentと合わさるような感じで配置します。
*  アイテムはプレハブ化しても良いけどシーンに配置しておくと調整が楽です。
*  Instantate Item Countは画面に映る数＋４(上に2、下に2）くらいで設定しておきます。  
（スクロール時に背景が見えてしまう為）
*  原則アイテムの高さは固定です。頑張れば可変にも出来ますが、対応予定は今のところありません。
*  ScrollRectのMovementTypeは無限ループする場合ScrollRect.MovementType.Unrestrictedに設定します。
