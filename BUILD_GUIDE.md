# ?? H??NG D?N BUILD VÀ XU?T GAME

## ?? M?c L?c
1. [Build Th? Công (Khuy?n Ngh? Cho Ng??i M?i)](#build-th?-công)
2. [Build T? ??ng (Nhanh)](#build-t?-??ng)
3. [Build Cho Các Platform Khác](#build-platform-khác)
4. [X? Lý L?i Build](#x?-lý-l?i-build)

---

## ?? BUILD TH? CÔNG (Khuy?n Ngh? Cho Ng??i M?i)

### B??c 1: Ki?m Tra Tr??c Khi Build

#### ? Checklist:
- [ ] T?t c? scenes ?ã ???c save (`Ctrl + S`)
- [ ] AudioManager ?ã ???c thêm vào scene MainMenu
- [ ] T?t c? AudioClips ?ã ???c gán vào AudioManager
- [ ] Game ch?y OK trong Unity Editor (nh?n Play ?? test)

### B??c 2: M? Build Settings

1. **File** ? **Build Settings...** (ho?c `Ctrl + Shift + B`)
2. C?a s? Build Settings hi?n ra

### B??c 3: Thêm Scenes Vào Build

**QUAN TR?NG!** Ph?i thêm ?? scenes:

#### Cách 1: Add Open Scenes
1. M? scene **MainMenu** trong Unity
2. Trong Build Settings, click **"Add Open Scenes"**
3. M? scene **Gameplay** (scene ch?i game)
4. Click **"Add Open Scenes"** l?n n?a

#### Cách 2: Drag & Drop
1. M? th? m?c `Assets/Scenes/` trong Project Window
2. Kéo **MainMenu** vào "Scenes In Build"
3. Kéo scene gameplay vào "Scenes In Build"

#### ? K?t qu?:
```
? Scenes/MainMenu           Index: 0  ? Scene ??u tiên (menu)
? Scenes/SampleScene        Index: 1  ? Scene gameplay
```

**L?u ý:** Scene có Index 0 s? ch?y ??u tiên!

### B??c 4: Ch?n Platform

#### Cho PC/Laptop (Khuy?n ngh?):
1. Ch?n **"PC, Mac & Linux Standalone"**
2. Target Platform: **Windows**
3. Architecture: **x86_64** (64-bit)

#### Cho ?i?n tho?i Android:
1. Ch?n **"Android"**
2. Click **"Switch Platform"** (??i Unity chu?n b?)

### B??c 5: C?u Hình Player Settings

Click nút **"Player Settings..."** ? d??i cùng:

#### ?? Company & Product (Tab Company Name):
```
Company Name: YourName
Product Name: Game2D Adventure
Version: 1.0
Default Icon: [Kéo icon game vào ?ây]
```

#### ??? Resolution (Tab Resolution and Presentation):
```
Fullscreen Mode: Fullscreen Window
Default Width: 1920
Default Height: 1080
? Resizable Window: Enabled
? Run In Background: Enabled
```

#### ?? Configuration (Tab Other Settings):
```
Scripting Backend: Mono
API Compatibility Level: .NET 4.x
Active Input Handling: Input Manager (Old)
```

### B??c 6: BUILD!

#### Quay l?i Build Settings:

| Nút | Khi Nào Dùng |
|-----|-------------|
| **Build** | Ch? build file, không ch?y ngay |
| **Build And Run** | Build xong t? ??ng ch?y game ?? test |

**Khuy?n ngh? l?n ??u:** Nh?n **"Build And Run"**

### B??c 7: Ch?n Th? M?c L?u

1. C?a s? ch?n th? m?c hi?n ra
2. **Navigate** ??n th? m?c project:
```
D:\LAPTRINH GAME\Game2dvippro\
```
3. **T?o th? m?c m?i**: `Builds`
4. Vào trong th? m?c `Builds`, t?o th? m?c `Windows`
5. Click **"Select Folder"**

### B??c 8: ??i Build Hoàn T?t

- Progress bar hi?n ra
- Th?i gian: **2-10 phút** (tùy máy)
- **KHÔNG T?T Unity** khi ?ang build!

### B??c 9: Ki?m Tra K?t Qu?

Sau khi build xong, m? th? m?c `Builds/Windows/`:

```
?? Windows\
 ?? ?? Game2D Adventure_Data\
 ?   ?? ?? Managed\           (Code .NET)
 ?   ?? ?? Resources\         (Assets, Audio, Sprites)
 ?   ?? ?? level0, level1...  (Scenes)
 ?? ?? MonoBleedingEdge\      (Runtime)
 ?? ?? Game2D Adventure.exe   ? CH?Y FILE NÀY!
 ?? ?? UnityCrashHandler64.exe
 ?? ?? UnityPlayer.dll
```

### ? Ch?y Game:
**Double click** vào `Game2D Adventure.exe` ? Game kh?i ??ng!

---

## ?? BUILD T? ??NG (Nhanh)

?ã t?o s?n script `BuildScript.cs` trong `Assets/Editor/`

### Cách Dùng:
1. Trong Unity Editor
2. Ch?n **Build** ? **Build Windows** (menu trên cùng)
3. Xong! File build t? ??ng xu?t ra `Builds/Windows/`

---

## ?? BUILD CHO CÁC PLATFORM KHÁC

### ?? Android (APK)

#### B??c 1: Switch Platform
1. Build Settings ? **Android**
2. Click **"Switch Platform"** (??i 5-10 phút)

#### B??c 2: C?u Hình Android
Player Settings ? Android tab:

```
Package Name: com.yourname.game2d
Version: 1.0
Version Code: 1
Minimum API Level: Android 5.0 (API 21)
Target API Level: Automatic (highest installed)
```

Keystore (Ký APK):
```
? Custom Keystore: [T?o keystore m?i]
Keystore Password: [M?t kh?u c?a b?n]
```

#### B??c 3: Build APK
1. Click **"Build"**
2. Ch?n th? m?c: `Builds/Android/`
3. ??t tên: `Game2D.apk`
4. ??i build xong (10-20 phút l?n ??u)

#### ? Cài APK:
- Copy file `Game2D.apk` vào ?i?n tho?i
- M? file và cài ??t
- Ch?y game!

---

### ?? WebGL (Ch?i Trên Trình Duy?t)

#### B??c 1: Switch Platform
1. Build Settings ? **WebGL**
2. Click **"Switch Platform"** (??i 10-15 phút l?n ??u)

#### B??c 2: Player Settings
```
Publishing Settings:
  Compression Format: Gzip (nh? h?n)
  Data Caching: Enabled
```

#### B??c 3: Build
1. Click **"Build"**
2. Ch?n th? m?c: `Builds/WebGL/`
3. ??i build (15-30 phút)

#### ? Test WebGL:
1. M? th? m?c `Builds/WebGL/`
2. Double click file `index.html`
3. Game ch?y trên trình duy?t!

#### ?? Upload Lên Web:
- Upload toàn b? th? m?c `WebGL` lên:
  - **itch.io** (mi?n phí, khuy?n ngh?)
  - **GitHub Pages**
  - Web hosting riêng

---

## ?? X? LÝ L?I BUILD

### L?i: "No scenes in build"
**Nguyên nhân:** Ch?a thêm scenes vào Build Settings  
**Gi?i pháp:** 
1. Build Settings ? Add Open Scenes
2. Thêm MainMenu scene (Index 0)
3. Thêm Gameplay scene (Index 1)

---

### L?i: "Building Library"... mãi không xong
**Nguyên nhân:** Unity ?ang import l?i assets  
**Gi?i pháp:** 
- ??i Unity import xong (có th? m?t 10-20 phút)
- Ki?m tra Console có l?i không

---

### L?i: Build b? crash ngay khi ch?y
**Nguyên nhân:** Thi?u AudioManager ho?c scenes không ?úng  
**Gi?i pháp:**
1. Ki?m tra AudioManager có trong MainMenu scene không
2. Ki?m tra scene index trong Build Settings
3. Test game trong Unity Editor tr??c khi build

---

### L?i: Không có âm thanh trong build
**Nguyên nhân:** Audio clips ch?a ???c gán ho?c import sai  
**Gi?i pháp:**
1. Ki?m tra Inspector c?a AudioManager
2. ??m b?o t?t c? AudioClips ?ã ???c gán
3. Ki?m tra Audio Import Settings:
   - Load Type: Decompress On Load (SFX)
   - Load Type: Streaming (Music)

---

### L?i: "Unable to parse Build/il2cpp/il2cpp..."
**Nguyên nhân:** IL2CPP backend ch?a cài ??t  
**Gi?i pháp:**
1. Player Settings ? Scripting Backend: **Mono**
2. Ho?c cài IL2CPP module qua Unity Hub

---

## ?? CHIA S? GAME

### Cho Windows:
**Cách 1: Nén thành ZIP**
1. Click chu?t ph?i vào th? m?c `Builds/Windows/`
2. **Send to** ? **Compressed (zipped) folder**
3. Chia s? file ZIP

**Cách 2: Dùng Installer (Nâng Cao)**
- Dùng **Inno Setup** ?? t?o file installer
- Ng??i ch?i ch? c?n double-click ?? cài ??t

### Upload Lên itch.io (Khuy?n Ngh?):
1. ??ng ký tài kho?n mi?n phí: https://itch.io/
2. Dashboard ? **Create New Project**
3. Upload file ZIP ho?c th? m?c build
4. ??t giá (mi?n phí ho?c tr? phí)
5. Publish!

---

## ? CHECKLIST BUILD HOÀN CH?NH

### Tr??c Khi Build:
- [ ] Game ch?y OK trong Editor
- [ ] T?t c? scenes ?ã save
- [ ] AudioManager ?ã ???c setup
- [ ] Scenes ?ã ???c thêm vào Build Settings
- [ ] Player Settings ?ã c?u hình ?úng

### Sau Khi Build:
- [ ] Test game build có ch?y không
- [ ] Ki?m tra âm thanh
- [ ] Ki?m tra menu + gameplay
- [ ] Test win/lose conditions
- [ ] Test restart game

### Tr??c Khi Chia S?:
- [ ] ?ã test trên máy khác
- [ ] Nén thành ZIP
- [ ] T?o README.txt h??ng d?n ch?i
- [ ] Ghi credit cho assets ?ã dùng

---

## ?? HOÀN T?T!

Bây gi? b?n ?ã có file game hoàn ch?nh!

**File quan tr?ng:**
- **`Game2D Adventure.exe`** ? Ch?y game
- **`Game2D Adventure_Data/`** ? D? li?u game (KHÔNG XÓA!)

**?? chia s?:**
- Nén th? m?c `Windows` thành ZIP
- Upload lên itch.io, Google Drive, ho?c g?i tr?c ti?p

**Chúc m?ng b?n ?ã hoàn thành game! ????**
