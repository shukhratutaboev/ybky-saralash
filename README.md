# YBKY dasturi uchun qilingan test topshiriq

Bu repository'da Impact.T coworking uchun qilingan xona band qilish dasturining backend qismining .NET 7 da yozilgan kodi bor.

## Qo'shimcha imkoniyatlar

* Dasturning muhim logikasini tekshirish uchun xUnit kutubxonasidan foydalangan holda testlar yozilgan.
* Kodni avtomatik tekshirib deploy qilish uchun `main` branch uchun GitHub Action yozilgan. Uning vazifalari:
  * Kodni test qilish.
  * Testdan muvaffaqiyatli o'tgandan so'ng `Dockerfile`dan foydalangan holda docker image yasab dockerhub'ga push qilish.
  * GitHub'ning self hosted runner'idan foydalangan holda Amazon EC2'ga ulanib, serverdagi `docker-compose.yml` file'ni run qilib, [quyidagi](https://ybky.shukhratutaboev.tech/swagger/index.html) domenga deploy qilish.
* Server tomonda reverse proxy sifatida Nginx ishlatilgan. HTTPS redirection uchun esa Let's encrypt SSL sertifikat ishlatilgan.

## Database haqida

Database sifatida PostgreSQL ishlatilgan. U serverda docker orqali ishlab turibti. Ma'lumotlar ishonchli saqlanishi uchun kerakli volume mount qilingan. Bizning holatda write'dan ko'ra read operatsiyalar ko'pligi sababli kerakli joylarda index'lar foydalanilgan.

## Database Model

### Table 1 - `rooms`

| Column | Description |
|-----|-----|
| `id` (PK) | `bigint` |
| `name` | `character varying(50)` |
| `type` | `character varying(50)` |
| `capacity` | `integer` |

### Table  - `booked_times`

| Column | Description |
|-----|-----|
| `id` (PK) | `bigint` |
| `room_id` (FK) | `bigint` |
| `resident` | `character varying(50)` |
| `start_time` | `timestamp without time zone` |
| `end_time` | `timestamp without time zone` |

#### Indexes
- Index 1: `start_time` (B+tree)
- Index 2: `end_time` (B+tree)

## Ishlatib ko'rish

Local ishlatib ko'rish uchun sizda docker bo'lishini o'zi yetarli.
Komandalar ketma-ketligi:
* Repository'ni local kompyuterga klon qilamiz: `git clone https://github.com/shukhratutaboev/ybky.git`
* Proyekt joylashgan folder ichiga kiramiz: `cd ybky`
* Agar kerak bo'lsa `docker-compose.yml` file ichida `postgresql` konfiguratsiyalarini o'zgartiramiz (port, password ...). Agar password'ni o'zgartirsak `impacttapi` da ham secret'dagi passwordni yangilab qo'yamiz.
* Compose file'ni run qilamiz: `docker compose up -d`
* Hamma containerlar ishga tushib olishini kutamiz, bu biroz vaqt olishi mumkin.
* Hammasi tugagandan so'ng `http://localhost:8080/swagger/index.html` address'ga borib application'ni tekshirib ko'rishimiz mumkin.

Savol va takliflar uchun bemalol [telegram](https://t.me/shukhrat_utaboev) orqali aloqaga chiqishingiz mumkin.
