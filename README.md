# Proiect - Programare Orientata pe Obiecte
## TI 2 IESC UNITBV
----------
Proiect realizat de Goia Ciprian

Continutul solutiei:
1. ProiectPOO - interfata aplicatiei
2. DatabaseCRUD - librarie dll in care se afla functiile pentru a lucra cu baza de date


Interfata este realizata cu ajutorul unei aplicatii de tip wpf(.net framework).

Conexiunea la baza de date este realizata cu Entity Framework.

Baza de date este hostata pe Microsoft Azure.Contine 3 tabele (Studenti, Discipline, Catalog) si procedurile de stocare pentru adaugare, modificarea, stergerea datelor.

Mai jos se afla tot codul pentru baza de date.

```sql

create table Studenti (
    NumarMatricol int not null constraint pk_Studenti primary key (NumarMatricol),
    Nume varchar(50) not null,
    Prenume varchar(50) not null,
    Sters tinyint constraint ck_sters check (Sters in (0, 1)) constraint df_implicit default (0)
)

create table Discipline (
    Cod_Disciplina varchar(50) constraint pk_Discipline primary key (Cod_Disciplina),
    Denumire varchar(50) not null ,
    NumarCredite int not null,
    Sters tinyint constraint ck_sters_discipline check (Sters in (0, 1)) constraint df_implicit_discipline default (0)
)

create table Catalog_Note (
    id bigint identity constraint pk_id_catalog primary key (id),
    Cod_Disciplina varchar(50) not null constraint fk_catre_discipline references Discipline(Cod_Disciplina),
    NumarMatricol int not null constraint fg_catre_student references Studenti(NumarMatricol),
    Nota int not null,
    Sters tinyint constraint ck_sters_catalog check (Sters in (0, 1)) constraint df_implicit_catalog default (0)
)

create or alter procedure CatalogDELETE
    @ID bigint
as
    set nocount on;
    declare @inserted table (
        id bigint,
        CodDisciplina varchar(50),
        NumarMatricol int,
        Nota int
    )
    begin transaction
        begin try
            update Catalog_Note set Sters = 1
            output inserted.id, inserted.Cod_Disciplina, inserted.NumarMatricol, inserted.Nota into @inserted
            where id = @ID;
            commit transaction ;
            select D.Cod_Disciplina, Denumire, NumarCredite, S.NumarMatricol, Nume, Prenume, id, Nota from @inserted i inner join Studenti S on i.NumarMatricol = S.NumarMatricol inner join Discipline D on i.CodDisciplina = D.Cod_Disciplina;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(150);
            set @errMessage = (select ERROR_MESSAGE());
            RAISERROR (@errMessage, 16, 1);
        end catch
go

create or alter procedure CatalogGET
as
    set nocount on;
    select D.Cod_Disciplina, Denumire, NumarCredite, C.NumarMatricol, Nume, Prenume, id, Nota from Catalog_Note C inner join Discipline D on D.Cod_Disciplina = C.Cod_Disciplina inner join Studenti S on S.NumarMatricol = C.NumarMatricol where C.Sters = 0;
go

create or alter procedure CatalogMERGE
    @ID int = null,
    @CodDisciplina varchar(50),
    @NumarMatricol int,
    @Nota int
as
    set nocount on;
    declare @inserted table (id bigint, Cod_Disciplina varchar(50), NumarMatricol int, Nota int);
    begin transaction
        begin try
            if(@ID in (select id from Catalog_Note))
            begin
                update Catalog_Note set Cod_Disciplina = @CodDisciplina, NumarMatricol = @NumarMatricol, Nota=@Nota, Sters = 0
                output inserted.id, inserted.Cod_Disciplina, inserted.NumarMatricol, inserted.Nota into @inserted
                where id = @ID;
            end
            else begin
                insert into Catalog_Note (Cod_Disciplina, NumarMatricol, Nota)
                output inserted.id, inserted.Cod_Disciplina, inserted.NumarMatricol, inserted.Nota into @inserted
                values (@CodDisciplina, @NumarMatricol, @Nota)
            end
            commit transaction
            select i.Cod_Disciplina, Denumire, NumarCredite, i.NumarMatricol, Nume, Prenume, id, Nota from @inserted i
                inner join Studenti s on i.NumarMatricol = s.NumarMatricol
                inner join Discipline d on i.Cod_Disciplina = d.Cod_Disciplina;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(150);
            set @errMessage = (select ERROR_MESSAGE());
            RAISERROR(@errMessage, 16, 1);
        end catch
go

create or alter procedure DisciplineDELETE
    @Cod_Disciplina varchar(50)
as
    set nocount on;
    declare @inserted table (CodDisciplina varchar(50), Denumire varchar(50), NumarCredite int)
    begin transaction
        begin try
            update Discipline set Sters = 1
            output inserted.Cod_Disciplina, inserted.Denumire, inserted.NumarCredite into @inserted
            where Cod_Disciplina = @Cod_Disciplina;
            commit transaction ;
            select CodDisciplina, Denumire, NumarCredite from @inserted;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(150);
            set @errMessage = (select ERROR_MESSAGE());
            RAISERROR(@errMessage, 16, 1);
        end catch
go

create or alter procedure DisciplineGET
as
    set nocount on;
    select Cod_Disciplina, Denumire, NumarCredite from Discipline where Sters = 0;
go

create or alter procedure DisciplineMERGE
    @CodDisciplina varchar(50),
    @Denumire varchar(50),
    @NumarCredite int
as
    set nocount on;
    declare @inserted table (Cod_Disciplina varchar(50), Denumire varchar(50), NumarCredite int);
    begin transaction
        begin try
            if(@CodDisciplina in (select Cod_Disciplina from Discipline))
            begin
                update Discipline set Denumire = @Denumire, NumarCredite = @NumarCredite, Sters = 0 output inserted.Cod_Disciplina, inserted.Denumire, inserted.NumarCredite into @inserted where Cod_Disciplina = @CodDisciplina;
            end
            else begin
                insert into Discipline (Cod_Disciplina, Denumire, NumarCredite) output inserted.Cod_Disciplina, inserted.Denumire, inserted.NumarCredite into @inserted values (@CodDisciplina, @Denumire, @NumarCredite);
            end
            commit transaction
            select Cod_Disciplina, Denumire, NumarCredite from @inserted;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(150);
            set @errMessage = (select  ERROR_MESSAGE());
            RAISERROR(@errMessage, 16, 1)
        end catch
go

create or alter procedure StudentiDELETE
    @NumarMatricol int
as
    declare @inserted table (NumarMatricol int, Nume varchar(50), Prenume varchar(50));
    begin transaction
        begin try
            update Studenti set Sters = 1
            output inserted.NumarMatricol, inserted.Nume, inserted.Prenume into @inserted
            where NumarMatricol = @NumarMatricol;
            commit transaction ;
            select NumarMatricol, Nume, Prenume from @inserted;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(150);
            set @errMessage = (select  ERROR_MESSAGE());
            RAISERROR(@errMessage, 16, 1)
        end catch
go

create or alter procedure StudentiGET
as
    set nocount on;
    select NumarMatricol, Nume, Prenume from Studenti where Sters = 0;
go

create or alter procedure StudentiMERGE
    @NumarMatricol int,
    @Nume varchar (50),
    @Prenume varchar(50)
as
    declare @inserted table (NumarMatricol int, Nume varchar(50), Prenume varchar(50))
    set nocount on
    begin transaction
        begin try
            if(@NumarMatricol in (select NumarMatricol from Studenti))
            begin
                update Studenti set Nume = @Nume, Prenume = @Prenume, Sters = 0
                output inserted.NumarMatricol, inserted.Nume, inserted.Prenume into @inserted where NumarMatricol = @NumarMatricol
            end
            else begin
                insert into Studenti (NumarMatricol, Nume, Prenume)
                output inserted.NumarMatricol, inserted.Nume, inserted.Prenume into @inserted values (@NumarMatricol, @Nume, @Prenume)
            end
            commit transaction
            select NumarMatricol, Nume,Prenume from @inserted;
        end try
        begin catch
            rollback transaction
            declare @errMessage varchar(50);
            set @errMessage = (select ERROR_MESSAGE());
            RAISERROR(@errMessage, 16, 1)
        end catch
go

exec StudentiMERGE 1, 'Cristian', 'Marius22'
exec DisciplineMERGE 'asd-231', 'Matematici speciale', 6
exec CatalogMERGE null, 'asd-231', 1, 10

exec StudentiMERGE 989, 'marika', 'Aur'
exec DisciplineMERGE 'asd-5', 'Teoria sistemelor', 6
exec CatalogMERGE null, 'asd-5', 2, 3

exec StudentiDELETE 22
exec DisciplineDELETE 'asd-5'
exec CatalogDELETE 3

select * from Studenti;

exec StudentiGET
exec DisciplineGET
exec CatalogGET

```

