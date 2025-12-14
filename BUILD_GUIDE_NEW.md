# 📖 HƯỚNG DẪN BUILD VÀ XUẤT GAME

## 📋 Mục Lục
1. [Build Thủ Công (Khuyến Nghị Cho Người Mới)](#build-thủ-công)
2. [Build Tự Động (Nhanh)](#build-tự-động)
3. [Build Cho Các Platform Khác](#build-platform-khác)
4. [Xử Lý Lỗi Build](#xử-lý-lỗi-build)

---

## 🔧 BUILD THỦ CÔNG (Khuyến Nghị Cho Người Mới)

### Bước 1: Kiểm Tra Trước Khi Build

#### ✅ Checklist:
- [ ] Tất cả scenes đã được save (`Ctrl + S`)
- [ ] AudioManager đã được thêm vào scene MainMenu
- [ ] Tất cả AudioClips đã được gán vào AudioManager
- [ ] Game chạy OK trong Unity Editor (nhấn Play để test)

### Bước 2: Mở Build Settings

1. **File** → **Build Settings...** (hoặc `Ctrl + Shift + B`)
2. Cửa sổ Build Settings hiện ra

### Bước 3: Thêm Scenes Vào Build

**QUAN TRỌNG!** Phải thêm đủ scenes:

#### Cách 1: Add Open Scenes
1. Mở scene **MainMenu** trong Unity
2. Trong Build Settings, click **"Add Open Scenes"**
3. Mở scene **Gameplay** (scene chơi game)
4. Click **"Add Open Scenes"** lần nữa

#### Cách 2: Drag & Drop
1. Mở thư mục `Assets/Scenes/` trong Project Window
2. Kéo **MainMenu** vào "Scenes In Build"
3. Kéo scene gameplay vào "Scenes In Build"

#### ✅ Kết quả:
```
✓ Scenes/MainMenu           Index: 0  → Scene đầu tiên (menu)
✓ Scenes/SampleScene        Index: 1  → Scene gameplay
```

**Lưu ý:** Scene có Index 0 sẽ chạy đầu tiên!

### Bước 4: Chọn Platform

#### Cho PC/Laptop (Khuyến nghị):
1. Chọn **"PC, Mac & Linux Standalone"**
2. Target Platform: **Windows**
3. Architecture: **x86_64** (64-bit)

#### Cho điện thoại Android:
1. Chọn **"Android"**
2. Click **"Switch Platform"** (đợi Unity chuẩn bị)

### Bước 5: Cấu Hình Player Settings

Click nút **"Player Settings..."** ở dưới cùng:

#### 🏢 Company & Product (Tab Company Name):
```
Company Name: YourName
Product Name: Game2D Adventure
Version: 1.0
Default Icon: [Kéo icon game vào đây]
```

#### 🖥️ Resolution (Tab Resolution and Presentation):
```
Fullscreen Mode: Fullscreen Window
Default Width: 1920
Default Height: 1080
✓ Resizable Window: Enabled
✓ Run In Background: Enabled
```

#### ⚙️ Configuration (Tab Other Settings):
```
Scripting Backend: Mono
API Compatibility Level: .NET 4.x
Active Input Handling: Input Manager (Old)
```

### Bước 6: BUILD!

#### Quay lại Build Settings:

| Nút | Khi Nào Dùng |
|-----|-------------|
| **Build** | Chỉ build file, không chạy ngay |
| **Build And Run** | Build xong tự động chạy game để test |

**Khuyến nghị lần đầu:** Nhấn **"Build And Run"**

### Bước 7: Chọn Thư Mục Lưu

1. Cửa sổ chọn thư mục hiện ra
2. **Navigate** đến thư mục project:
```
D:\LAPTRINH GAME\Game2dvippro\
```
3. **Tạo thư mục mới**: `Builds`
4. Vào trong thư mục `Builds`, tạo thư mục `Windows`
5. Click **"Select Folder"**

### Bước 8: Đợi Build Hoàn Tất

- Progress bar hiện ra
- Thời gian: **2-10 phút** (tùy máy)
- **KHÔNG TẮT Unity** khi đang build!

### Bước 9: Kiểm Tra Kết Quả

Sau khi build xong, mở thư mục `Builds/Windows/`:

```
📁 Windows\
 ├─ 📁 Game2D Adventure_Data\
 │   ├─ 📁 Managed\           (Code .NET)
 │   ├─ 📁 Resources\         (Assets, Audio, Sprites)
 │   └─ 📁 level0, level1...  (Scenes)
 ├─ 📁 MonoBleedingEdge\      (Runtime)
 ├─ 📄 Game2D Adventure.exe   → CHẠY FILE NÀY!
 ├─ 📄 UnityCrashHandler64.exe
 └─ 📄 UnityPlayer.dll
```

### ▶ Chạy Game:
**Double click** vào `Game2D Adventure.exe` → Game khởi động!

---

## 🚀 BUILD TỰ ĐỘNG (Nhanh)

Đã tạo sẵn script `BuildScript.cs` trong `Assets/Editor/`

### Cách Dùng:
1. Trong Unity Editor
2. Chọn **Build** → **Build Windows** (menu trên cùng)
3. Xong! File build tự động xuất ra `Builds/Windows/`

---

## 📱 BUILD CHO CÁC PLATFORM KHÁC

### 📱 Android (APK)

#### Bước 1: Switch Platform
1. Build Settings → **Android**
2. Click **"Switch Platform"** (đợi 5-10 phút)

#### Bước 2: Cấu Hình Android
Player Settings → Android tab:

```
Package Name: com.yourname.game2d
Version: 1.0
Version Code: 1
Minimum API Level: Android 5.0 (API 21)
Target API Level: Automatic (highest installed)
```

Keystore (Ký APK):
```
✓ Custom Keystore: [Tạo keystore mới]
Keystore Password: [Mật khẩu của bạn]
```

#### Bước 3: Build APK
1. Click **"Build"**
2. Chọn thư mục: `Builds/Android/`
3. Đặt tên: `Game2D.apk`
4. Đợi build xong (10-20 phút lần đầu)

#### ✓ Cài APK:
- Copy file `Game2D.apk` vào điện thoại
- Mở file và cài đặt
- Chạy game!

---

### 🌐 WebGL (Chơi Trên Trình Duyệt)

#### Bước 1: Switch Platform
1. Build Settings → **WebGL**
2. Click **"Switch Platform"** (đợi 10-15 phút lần đầu)

#### Bước 2: Player Settings
```
Publishing Settings:
  Compression Format: Gzip (nhẹ hơn)
  Data Caching: Enabled
```

#### Bước 3: Build
1. Click **"Build"**
2. Chọn thư mục: `Builds/WebGL/`
3. Đợi build (15-30 phút)

#### ✓ Test WebGL:
1. Mở thư mục `Builds/WebGL/`
2. Double click file `index.html`
3. Game chạy trên trình duyệt!

#### 🌍 Upload Lên Web:
- Upload toàn bộ thư mục `WebGL` lên:
  - **itch.io** (miễn phí, khuyến nghị)
  - **GitHub Pages**
  - Web hosting riêng

---

## 🔧 XỬ LÝ LỖI BUILD

### Lỗi: "No scenes in build"
**Nguyên nhân:** Chưa thêm scenes vào Build Settings  
**Giải pháp:** 
1. Build Settings → Add Open Scenes
2. Thêm MainMenu scene (Index 0)
3. Thêm Gameplay scene (Index 1)

---

### Lỗi: "Building Library"... mãi không xong
**Nguyên nhân:** Unity đang import lại assets  
**Giải pháp:** 
- Đợi Unity import xong (có thể mất 10-20 phút)
- Kiểm tra Console có lỗi không

---

### Lỗi: Build bị crash ngay khi chạy
**Nguyên nhân:** Thiếu AudioManager hoặc scenes không đúng  
**Giải pháp:**
1. Kiểm tra AudioManager có trong MainMenu scene không
2. Kiểm tra scene index trong Build Settings
3. Test game trong Unity Editor trước khi build

---

### Lỗi: Không có âm thanh trong build
**Nguyên nhân:** Audio clips chưa được gán hoặc import sai  
**Giải pháp:**
1. Kiểm tra Inspector của AudioManager
2. Đảm bảo tất cả AudioClips đã được gán
3. Kiểm tra Audio Import Settings:
   - Load Type: Decompress On Load (SFX)
   - Load Type: Streaming (Music)

---

### Lỗi: "Unable to parse Build/il2cpp/il2cpp..."
**Nguyên nhân:** IL2CPP backend chưa cài đặt  
**Giải pháp:**
1. Player Settings → Scripting Backend: **Mono**
2. Hoặc cài IL2CPP module qua Unity Hub

---

## 📦 CHIA SẺ GAME

### Cho Windows:
**Cách 1: Nén thành ZIP**
1. Click chuột phải vào thư mục `Builds/Windows/`
2. **Send to** → **Compressed (zipped) folder**
3. Chia sẻ file ZIP

**Cách 2: Dùng Installer (Nâng Cao)**
- Dùng **Inno Setup** để tạo file installer
- Người chơi chỉ cần double-click để cài đặt

### Upload Lên itch.io (Khuyến Nghị):
1. Đăng ký tài khoản miễn phí: https://itch.io/
2. Dashboard → **Create New Project**
3. Upload file ZIP hoặc thư mục build
4. Đặt giá (miễn phí hoặc trả phí)
5. Publish!

---

## ✅ CHECKLIST BUILD HOÀN CHỈNH

### Trước Khi Build:
- [ ] Game chạy OK trong Editor
- [ ] Tất cả scenes đã save
- [ ] AudioManager đã được setup
- [ ] Scenes đã được thêm vào Build Settings
- [ ] Player Settings đã cấu hình đúng

### Sau Khi Build:
- [ ] Test game build có chạy không
- [ ] Kiểm tra âm thanh
- [ ] Kiểm tra menu + gameplay
- [ ] Test win/lose conditions
- [ ] Test restart game

### Trước Khi Chia Sẻ:
- [ ] Đã test trên máy khác
- [ ] Nén thành ZIP
- [ ] Tạo README.txt hướng dẫn chơi
- [ ] Ghi credit cho assets đã dùng

---

## 🎉 HOÀN TẤT!

Bây giờ bạn đã có file game hoàn chỉnh!

**File quan trọng:**
- **`Game2D Adventure.exe`** → Chạy game
- **`Game2D Adventure_Data/`** → Dữ liệu game (KHÔNG XÓA!)

**Để chia sẻ:**
- Nén thư mục `Windows` thành ZIP
- Upload lên itch.io, Google Drive, hoặc gửi trực tiếp

**Chúc mừng bạn đã hoàn thành game! 🎮🎉**
