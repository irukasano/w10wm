# w10wm ( windows10 window manager)

手習い用 C# でwindows10に親和性のあるダイナミックウィンドウマネージャーを作ってみる

## 前提

* いまのところ UWP アプリケーションは管理下におかないほうがよさそう（WindowLongの値が普通のアプリケーションとは違っていてよくわからないから）

## ショートカットキー（Windows10 標準）

* Winキー + Shift + → : アクティヴウィンドウを右ディスプレーに移動
* Winキー + Shift + ← : アクティヴウィンドウを左ディスプレーに移動
* Winキー + Ctl + → : 次の仮想デスクトップを表示
* Winキー + Ctl + → : 前の仮想デスクトップを表示
* Winキー + → : アクティヴウィンドウをアクティヴディスプレー内で右寄せする
* Winキー + ← : アクティヴウィンドウをアクティヴディスプレー内で左寄せする
* Winキー + ↑ : アクティヴウィンドウを最大化する
* Winキー + ↓ : アクティヴウィンドウを最小化する

## 追加した機能

* Winキー + F1 : 番号１のディスプレーをアクティヴにする（アクティヴディスプレーをなんかハイライトしたい）
* Winキー + F2 : 番号２のディスプレーをアクティヴにする（アクティヴディスプレーをなんかハイライトしたい）
* Winキー + F3 : 番号３のディスプレーをアクティヴにする（アクティヴディスプレーをなんかハイライトしたい）
* Winキー + c : アクティヴディスプレーを一瞬ハイライト表示する

* Winキー + . : 右のディスプレーをアクティヴにする（アクティヴディスプレーをなんかハイライトしたい）
* Winキー + , : 左のディスプレーをアクティヴにする（アクティヴディスプレーをなんかハイライトしたい）

* Winキー + j : 現在のアクティヴディスプレー内でアクティヴウィンドウのひとつ上のウィンドウにフォーカスを移動
* Winキー + k : 現在のアクティヴディスプレー内でアクティヴウィンドウのひとつ下のウィンドウにフォーカスを移動

* Winキー + f : 現在のアクティヴディスプレー内の全てのウィンドウを全画面表示する
* Winキー + g : 現在のアクティヴディスプレー内の全てのウィンドウを４分割表示する
* Winキー + t : 現在のアクティヴディスプレー内の全てのウィンドウをタイル表示する
* Winキー + r : 現在のアクティヴディスプレー内の全てのウィンドウを通常画面表示する(MDI的な)
* Winキー + n : 現在のアクティヴディスプレー内の全てのウィンドウを整列しない

* Winキー + x : 現在のアクティヴウィンドウを閉じる


## 最初に追加したい機能

* Winキー + Shift + F1 : アクティヴウィンドウを番号１のディスプレーに移動
* Winキー + Shift + F2 : アクティヴウィンドウを番号２のディスプレーに移動
* Winキー + Shift + F3 : アクティヴウィンドウを番号３のディスプレーに移動

* Winキー + Shift + . : アクティヴウィンドウを右のディスプレーに移動
* Winキー + Shift + , : アクティヴウィンドウを左のディスプレーに移動

* Winキー + u : 現在のアクティヴディスプレー内で一番上のウィンドウにフォーカスを移動
* Winキー + m : 現在のアクティヴディスプレー内で一番下のウィンドウにフォーカスを移動

* Winキー + Sift + j : 現在のアクティヴディスプレー内でアクティヴウィンドウをひとつ上に移動
* Winキー + Sift + k : 現在のアクティヴディスプレー内でアクティヴウィンドウをひとつ下に移動

* Winキー + Sift + u : 現在のアクティヴディスプレー内でアクティヴウィンドウを一番上に移動
* Winキー + Sift + m : 現在のアクティヴディスプレー内でアクティヴウィンドウを一番下に移動


* Winキー + i : 現在のアクティヴウィンドウの情報を表示（hWndとかWindowLongの値とか？）


## 将来追加したい機能

* Winキー + Shift + 1～9 : 現在のアクティヴウィンドウを仮想デスクトップ1～9に移動
* Winキー + Shift + Ctl + 1～9 : 現在のアクティヴディスプレー内の全てのウィンドウを、仮想デスクトップ1～9に移動

* Winキー + Ctl + → (?) : （タイル表示モードのみ）右側の枠を少し広げる
* Winキー + Ctl + ← (?) : （タイル表示モードのみ）右側の枠を少し狭める

* Winキー + Shift + e (?): 現在のアクティヴウィンドウを管理対象外にする
* Winキー + Shift + j (?): 現在のアクティヴウィンドウを管理対象化におく


